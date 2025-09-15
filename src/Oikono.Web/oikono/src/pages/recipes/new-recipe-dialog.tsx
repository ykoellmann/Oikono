import * as React from "react";
import {Button} from "@/components/ui/button";
import {FormLabel, FormMessage} from "@/components/ui/form";
import {Plus, Trash2, ArrowUp, ArrowDown, ImagePlus} from "lucide-react";
import {useForm, useFieldArray} from "react-hook-form";
import {z} from "zod";
import {zodResolver} from "@hookform/resolvers/zod";
import {RecipeService} from "@/pages/recipes/lib/recipeService";
import {ScrollArea} from "@/components/ui/scroll-area";
import {
    Popup,
    PopupClose,
    PopupContent,
    PopupDescription,
    PopupFooter,
    PopupHeader,
    PopupTitle,
    PopupTrigger
} from "@/components/ui/popup";
import {Input} from "@/components/ui/input.tsx";
import {Textarea} from "@/components/ui/textarea.tsx";
import { TagBoxWithCreate } from "@/components/tag-box-with-create";

const IngredientSchema = z.object({
    name: z.string().min(1, "Required").max(80),
    amount: z.coerce.number().min(0.001, "> 0"),
    unit: z.string().max(20).optional().default("")
});

const StepSchema = z.object({
    content: z.string().min(1, "Required")
});

const RecipeCreateSchema = z.object({
    name: z.string().min(2).max(120),
    portions: z.coerce.number().int().min(1).max(50),
    tags: z.string().optional().default(""),
    ingredients: z.array(IngredientSchema).min(1, "At least one ingredient"),
    steps: z.array(StepSchema).min(1, "At least one step"),
    images: z.any().optional(),
});

type FormValues = z.infer<typeof RecipeCreateSchema>;

function CreateRecipeForm({onSaved}: { onSaved?: () => void }) {
    const [submitError, setSubmitError] = React.useState<string | null>(null);
    const [previews, setPreviews] = React.useState<string[]>([]);

    const form = useForm<FormValues>({
        resolver: zodResolver(RecipeCreateSchema),
        defaultValues: {
            name: "",
            portions: 2,
            tags: "",
            ingredients: [{name: "", amount: 1, unit: ""}],
            steps: [{content: ""}],
        },
        mode: "onTouched",
    });

    const ingArray = useFieldArray({control: form.control, name: "ingredients"});
    const stepsArray = useFieldArray({control: form.control, name: "steps"});

    // Tags options for TagBoxWithCreate
    type TagOption = { label: string; value: string };
    const [tagOptions, setTagOptions] = React.useState<TagOption[]>([]);

    React.useEffect(() => {
        let active = true;
        RecipeService.getTags()
            .then(tags => {
                if (!active) return;
                const opts = tags.map(t => ({ label: t, value: t }));
                setTagOptions(opts);
            })
            .catch(() => {/* ignore */});
        return () => { active = false; };
    }, []);

    const tagsStr = form.watch("tags");
    const selectedTags = React.useMemo(() => (tagsStr ?? "")
        .split(",")
        .map(t => t.trim())
        .filter(Boolean), [tagsStr]);

    async function handleCreateTag(value: string) {
        const v = (value || "").trim();
        if (!v) return;
        try {
            await RecipeService.upsertTag(v);
            setTagOptions(prev => {
                if (prev.some(o => o.value === v)) return prev;
                return [...prev, { label: v, value: v }];
            });
        } catch {
            // ignore errors
        }
    }

    function onFilesChange(files: FileList | null) {
        const arr = files ? Array.from(files) : [];
        setPreviews(arr.map(f => URL.createObjectURL(f)));
        form.setValue("images", arr as unknown as File[]);
    }

    function moveStep(index: number, dir: -1 | 1) {
        const target = index + dir;
        if (target < 0 || target >= stepsArray.fields.length) return;
        stepsArray.move(index, target);
    }

    async function onSubmit(values: FormValues) {
        setSubmitError(null);
        try {
            const fd = new FormData();
            fd.append("name", values.name);
            fd.append("portions", String(values.portions));
            const tags = (values.tags ?? "").split(",").map(t => t.trim()).filter(Boolean);
            fd.append("tags", JSON.stringify(tags));
            fd.append("ingredients", JSON.stringify(values.ingredients));
            const steps = values.steps.map((s, i) => ({step: s.content, order: i}));
            fd.append("steps", JSON.stringify(steps));
            const imgs = (values.images as unknown as File[]) || [];
            imgs.forEach((f, i) => fd.append("images", f, f.name || `image-${i}.jpg`));

            await RecipeService.create(fd);
            onSaved?.();
        } catch (e) {
            const err = e as { response?: { data?: { title?: string; errors?: Record<string, string[]> } } };
            setSubmitError(err?.response?.data?.title || "Speichern fehlgeschlagen");
        }
    }

    return (
        <form id="create-recipe-form" className="flex flex-col gap-6" onSubmit={form.handleSubmit(onSubmit)}>
            <section className="grid gap-3">
                <FormLabel className="text-left" htmlFor="name" invalid={!!form.formState.errors.name}>Titel</FormLabel>
                <Input id="name" {...form.register("name")} placeholder="z. B. Spaghetti Bolognese"/>
                <FormMessage className="text-left">{form.formState.errors.name?.message as string}</FormMessage>
            </section>

            <div className="grid grid-cols-2 gap-4">
                <section className="grid gap-3">
                    <FormLabel className="text-left" htmlFor="portions"
                               invalid={!!form.formState.errors.portions}>Portionen</FormLabel>
                    <Input id="portions" type="number" min={1}
                           max={50} {...form.register("portions", {valueAsNumber: true})} />
                    <FormMessage className="text-left">{form.formState.errors.portions?.message as string}</FormMessage>
                </section>
                <section className="grid gap-3">
                    <FormLabel className="text-left" htmlFor="tags">Tags</FormLabel>
                    <TagBoxWithCreate
                        options={tagOptions}
                        values={selectedTags}
                        onChange={(vals) => form.setValue("tags", vals.join(", "))}
                        onCreate={handleCreateTag}
                        placeholder="Tag auswählen oder neu erstellen"
                    />
                </section>
            </div>

            <section className="grid gap-3">
                <FormLabel className="text-left">Bilder</FormLabel>
                <div className="flex items-center gap-3">
                    <label className="inline-flex items-center gap-2 px-3 py-2 border rounded cursor-pointer">
                        <ImagePlus className="h-4 w-4"/>
                        <span>Bild hochladen</span>
                        <input className="hidden" type="file" accept="image/*" multiple
                               onChange={e => onFilesChange(e.target.files)}/>
                    </label>
                </div>
                {previews.length > 0 && (
                    <div className="grid grid-cols-3 gap-2">
                        {previews.map((src, i) => (
                            <img key={i} src={src} alt={`preview-${i}`}
                                 className="aspect-video w-full object-cover rounded border"/>
                        ))}
                    </div>
                )}
            </section>

            <section className="grid gap-3">
                <FormLabel className="text-left" invalid={!!form.formState.errors.ingredients}>Zutaten</FormLabel>
                <div className="flex flex-col gap-2">
                    {ingArray.fields.map((field, idx) => (
                        <div key={field.id} className="grid grid-cols-[1fr_auto_auto_auto] gap-2 items-center">
                            <Input placeholder="Zutat" {...form.register(`ingredients.${idx}.name` as const)} />
                            <Input placeholder="Menge" type="number" step="0.01"
                                   className="w-28" {...form.register(`ingredients.${idx}.amount` as const)} />
                            <Input placeholder="Einheit"
                                   className="w-28" {...form.register(`ingredients.${idx}.unit` as const)} />
                            <Button type="button" variant="ghost" size="icon" onClick={() => ingArray.remove(idx)}>
                                <Trash2 className="h-4 w-4"/>
                            </Button>
                        </div>
                    ))}
                    <Button type="button" variant="outline"
                            onClick={() => ingArray.append({name: "", amount: 1, unit: ""})}>
                        <Plus className="h-4 w-4 mr-2"/> Zutat hinzufügen
                    </Button>
                </div>
                <FormMessage className="text-left">{form.formState.errors.ingredients?.message as string}</FormMessage>
            </section>

            <section className="grid gap-3">
                <FormLabel className="text-left" invalid={!!form.formState.errors.steps}>Schritte</FormLabel>
                <div className="flex flex-col gap-3">
                    {stepsArray.fields.map((field, idx) => (
                        <div key={field.id} className="border rounded p-3 space-y-2">
                            <div className="flex gap-1 justify-end">
                                <Button type="button" variant="ghost" size="icon" onClick={() => moveStep(idx, -1)}
                                        disabled={idx === 0}><ArrowUp className="h-4 w-4"/></Button>
                                <Button type="button" variant="ghost" size="icon" onClick={() => moveStep(idx, 1)}
                                        disabled={idx === stepsArray.fields.length - 1}><ArrowDown className="h-4 w-4"/></Button>
                                <Button type="button" variant="ghost" size="icon"
                                        onClick={() => stepsArray.remove(idx)}><Trash2 className="h-4 w-4"/></Button>
                            </div>
                            <Textarea rows={4}
                                      placeholder={`Schritt ${idx + 1} beschreiben…`} {...form.register(`steps.${idx}.content` as const)} />
                        </div>
                    ))}
                    <Button type="button" variant="outline" onClick={() => stepsArray.append({content: ""})}>
                        <Plus className="h-4 w-4 mr-2"/> Schritt hinzufügen
                    </Button>
                </div>
                <FormMessage className="text-left">{form.formState.errors.steps?.message as string}</FormMessage>
            </section>

            {submitError && <div className="text-sm text-red-600">{submitError}</div>}
        </form>
    );
}

export function CreateRecipeDialog({children, onCreated}: { children: React.ReactNode; onCreated?: () => void }) {
    const [open, setOpen] = React.useState(false);

    function handleSaved() {
        setOpen(false);
        onCreated?.();
        window.location.reload();
    }

    return (
        <Popup open={open} onOpenChange={setOpen}>
            <PopupTrigger asChild>{children}</PopupTrigger>
            <PopupContent className="sm:max-w-[720px] p-0 flex flex-col h-[80vh]">
                <PopupHeader className="px-6 pt-6">
                    <PopupTitle>Neues Rezept</PopupTitle>
                    <PopupDescription>Erstelle ein neues Rezept und speichere es.</PopupDescription>
                </PopupHeader>

                {/* Scrollbarer Bereich */}
                <ScrollArea className="flex-1 min-h-0 px-6 pb-6">
                    <CreateRecipeForm onSaved={handleSaved}/>

                    <PopupFooter className="px-6 pb-6">
                        <PopupClose asChild>
                            <Button variant="outline">Schließen</Button>
                        </PopupClose>
                        <Button type="submit" form="create-recipe-form">
                            Speichern
                        </Button>
                    </PopupFooter>
                </ScrollArea>
            </PopupContent>
        </Popup>
    );
}
