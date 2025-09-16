import * as React from "react";
import { Button } from "@/components/ui/button";
import { FormLabel, FormMessage } from "@/components/ui/form";
import { Plus, Trash2, ArrowUp, ArrowDown, ImagePlus, Utensils } from "lucide-react";
import { useForm, useFieldArray } from "react-hook-form";
import { z } from "zod";
import { zodResolver } from "@hookform/resolvers/zod";
import { RecipeService } from "@/pages/recipes/lib/recipeService";
import { ScrollArea } from "@/components/ui/scroll-area";
import { Popup, PopupClose, PopupContent, PopupDescription, PopupFooter, PopupHeader, PopupTitle, PopupTrigger } from "@/components/ui/popup";
import { Input } from "@/components/ui/input.tsx";
import { Textarea } from "@/components/ui/textarea.tsx";
import { TagBoxWithCreate } from "@/components/tag-box-with-create";
import { TagService } from "@/api/tagService";
import { SideDishService } from "@/api/sideDishService";
import { IngredientService } from "@/api/ingredientService";
import { UnitService } from "@/api/unitService";
import { DeviceService } from "@/api/deviceService";
import { SelectBoxWithCreate, type Option } from "@/components/select-box-with-create";

// Schema aligned to backend CreateRecipeRequest
const IngredientRowSchema = z.object({
  ingredientId: z.string().uuid({ message: "Zutat wählen" }),
  amount: z.coerce.number().min(0.001, "> 0"),
  unit: z.coerce.number().int(), // UnitType enum int from /units
});

const PartSchema = z.object({
  name: z.string().min(1, "Name erforderlich"),
  ingredients: z.array(IngredientRowSchema).min(1, "Mind. 1 Zutat"),
});

const StepSchema = z.object({
  description: z.string().min(1, "Erforderlich"),
  durationMinutes: z.coerce.number().int().min(0).max(24 * 60).optional().nullable(),
  temperature: z.coerce.number().int().min(0).max(400).optional().nullable(),
  deviceId: z.string().uuid().optional().nullable(),
});

const RecipeCreateSchema = z.object({
  name: z.string().min(2).max(120),
  portions: z.coerce.number().int().min(1).max(50),
  calories: z.coerce.number().int().min(0).max(5000).optional().nullable(),
  rating: z.coerce.number().int().min(0).max(5).optional().nullable(),
  tagIds: z.array(z.string().uuid()).optional().default([]),
  sideDishIds: z.array(z.string().uuid()).optional().default([]),
  parts: z.array(PartSchema).optional().default([]),
  steps: z.array(StepSchema).min(1, "Mind. 1 Schritt"),
  images: z.any().optional(),
});

type FormValues = z.infer<typeof RecipeCreateSchema>;

function RatingPicker({ value, onChange }: { value: number; onChange: (v: number) => void }) {
  return (
    <div className="flex items-center gap-2">
      {Array.from({ length: 5 }).map((_, i) => {
        const filled = i < (value ?? 0);
        return (
          <button type="button" key={i} onClick={() => onChange(i + 1)} className="p-0">
            <Utensils className={`w-5 h-5 ${filled ? 'text-amber-500' : 'text-muted-foreground/40'}`} />
          </button>
        );
      })}
      <button type="button" className="text-xs text-muted-foreground underline" onClick={() => onChange(0)}>Zurücksetzen</button>
    </div>
  );
}

function CreateRecipeForm({ onSaved }: { onSaved?: () => void }) {
  const [submitError, setSubmitError] = React.useState<string | null>(null);
  const [previews, setPreviews] = React.useState<string[]>([]);
  const [files, setFiles] = React.useState<File[]>([]);

  const form = useForm<FormValues>({
    resolver: zodResolver(RecipeCreateSchema),
    defaultValues: {
      name: "",
      portions: 2,
      calories: null,
      rating: 0,
      tagIds: [],
      sideDishIds: [],
      parts: [],
      steps: [{ description: "", durationMinutes: null, temperature: null, deviceId: null }],
    },
    mode: "onTouched",
  });

  // Field arrays
  const partsArray = useFieldArray({ control: form.control, name: "parts" });
  const stepsArray = useFieldArray({ control: form.control, name: "steps" });

  // Lookups
  const [tagOptions, setTagOptions] = React.useState<Option<string>[]>([]);
  const [sideDishOptions, setSideDishOptions] = React.useState<Option<string>[]>([]);
  const [ingredientOptions, setIngredientOptions] = React.useState<Option<string>[]>([]);
  const [unitOptions, setUnitOptions] = React.useState<Option<number>[]>([]); // value: number
  const [deviceOptions, setDeviceOptions] = React.useState<Option<string>[]>([]);

  React.useEffect(() => {
    let active = true;
    (async () => {
      try {
        const [tags, sides, ings, units, devices] = await Promise.all([
          TagService.listLookups(),
          SideDishService.listLookups().catch(() => []),
          IngredientService.listLookups().catch(() => []),
          UnitService.listOptions().catch(() => []),
          DeviceService.listLookups().catch(() => []),
        ]);
        if (!active) return;
        setTagOptions(tags.map(t => ({ label: t.name, value: t.id })));
        setSideDishOptions(sides.map(s => ({ label: s.name, value: s.id })));
        setIngredientOptions(ings.map(i => ({ label: i.name, value: i.id })));
        setUnitOptions(units);
        setDeviceOptions(devices.map(d => ({ label: d.name, value: d.id })));
      } catch {
        // ignore
      }
    })();
    return () => { active = false; };
  }, []);

  async function onCreateTag(label: string) {
    const created = await TagService.createLookup(label);
    const opt = { label: created.name, value: created.id } as Option<string>;
    setTagOptions(prev => prev.some(o => o.value === opt.value) ? prev : [...prev, opt]);
  }

  async function onCreateSideDish(label: string) {
    const created = await SideDishService.createLookup(label);
    const opt = { label: created.name, value: created.id } as Option<string>;
    setSideDishOptions(prev => prev.some(o => o.value === opt.value) ? prev : [...prev, opt]);
    form.setValue("sideDishIds", [...(form.getValues("sideDishIds") ?? []), opt.value]);
  }

  async function onCreateIngredient(label: string) {
    const created = await IngredientService.createLookup(label);
    const opt = { label: created.name, value: created.id } as Option<string>;
    setIngredientOptions(prev => prev.some(o => o.value === opt.value) ? prev : [...prev, opt]);
  }


  async function onCreateDevice(label: string) {
    const created = await DeviceService.createLookup(label);
    const opt = { label: created.name, value: created.id } as Option<string>;
    setDeviceOptions(prev => prev.some(o => o.value === opt.value) ? prev : [...prev, opt]);
  }

  function onFilesChange(list: FileList | null) {
    const incoming = list ? Array.from(list) : [];
    // append to existing files
    const nextFiles = [...files, ...incoming];
    setFiles(nextFiles);
    // add previews for new files only
    const newPreviews = incoming.map(f => URL.createObjectURL(f));
    setPreviews(prev => [...prev, ...newPreviews]);
    form.setValue("images", nextFiles as unknown as File[]);
  }

  function removeImageAt(index: number) {
    setFiles(prev => {
      const next = prev.slice();
      next.splice(index, 1);
      form.setValue("images", next as unknown as File[]);
      return next;
    });
    setPreviews(prev => {
      const next = prev.slice();
      // Revoke the object URL to avoid memory leaks
      try { URL.revokeObjectURL(next[index]); } catch {}
      next.splice(index, 1);
      return next;
    });
  }

  function moveStep(index: number, dir: -1 | 1) {
    const target = index + dir;
    if (target < 0 || target >= stepsArray.fields.length) return;
    stepsArray.move(index, target);
  }

  function minutesToTimeSpan(mins?: number | null): string | undefined {
    if (mins == null) return undefined;
    const m = Math.max(0, Math.min(24 * 60, Math.floor(mins)));
    const hh = String(Math.floor(m / 60)).padStart(2, "0");
    const mm = String(m % 60).padStart(2, "0");
    return `${hh}:${mm}:00`;
  }

  function PartEditor({ pIdx, onRemove }: { pIdx: number; onRemove: () => void }) {
    const ingArray = useFieldArray({ control: form.control, name: `parts.${pIdx}.ingredients` as const });
    return (
      <div className="border rounded p-3 space-y-3">
        <div className="flex items-center gap-2">
          <Input placeholder="Komponentenname" {...form.register(`parts.${pIdx}.name` as const)} />
          <Button type="button" variant="ghost" size="icon" onClick={onRemove}>
            <Trash2 className="h-4 w-4" />
          </Button>
        </div>
        <div className="flex flex-col gap-2">
          {ingArray.fields.map((ing, iIdx) => (
            <div key={ing.id} className="grid grid-cols-[1fr_auto_auto_auto] gap-2 items-center">
              <SelectBoxWithCreate<string>
                options={ingredientOptions}
                value={(form.watch(`parts.${pIdx}.ingredients.${iIdx}.ingredientId` as const) as unknown as string) ?? null}
                onChange={(v) => form.setValue(`parts.${pIdx}.ingredients.${iIdx}.ingredientId` as const, (v ?? ""))}
                onCreate={onCreateIngredient}
                placeholder="Zutat wählen"
              />
              <Input type="number" step="0.01" className="w-28" placeholder="Menge"
                     {...form.register(`parts.${pIdx}.ingredients.${iIdx}.amount` as const, { valueAsNumber: true })} />
              <SelectBoxWithCreate<number>
                options={unitOptions}
                value={(form.watch(`parts.${pIdx}.ingredients.${iIdx}.unit` as const) as unknown as number) ?? null}
                onChange={(v) => form.setValue(`parts.${pIdx}.ingredients.${iIdx}.unit` as const, (v as number) ?? (undefined as unknown as number))}
                allowCreate={false}
                placeholder="Einheit"
              />
              <Button type="button" variant="ghost" size="icon" onClick={() => ingArray.remove(iIdx)}>
                <Trash2 className="h-4 w-4" />
              </Button>
            </div>
          ))}
          <Button type="button" variant="outline" onClick={() => ingArray.append({ ingredientId: "", amount: 1, unit: NaN as unknown as number })}>
            <Plus className="h-4 w-4 mr-2" /> Zutat hinzufügen
          </Button>
        </div>
      </div>
    );
  }

  async function onSubmit(values: FormValues) {
    setSubmitError(null);
    try {
      const payload = {
        name: values.name,
        portions: values.portions,
        calories: values.calories ?? undefined,
        rating: values.rating ?? undefined,
        tags: values.tagIds ?? [],
        sideDishes: values.sideDishIds ?? [],
        parts: (values.parts ?? []).map(p => ({
          name: p.name,
          ingredients: p.ingredients.map(i => ({
            ingredientId: i.ingredientId,
            amount: i.amount,
            unit: i.unit, // UnitType enum int
          }))
        })),
        steps: values.steps.map(s => ({
          description: s.description,
          duration: minutesToTimeSpan(s.durationMinutes ?? undefined),
          temperature: s.temperature ?? undefined,
          deviceId: s.deviceId ?? undefined,
        })),
      };

      await RecipeService.create(payload as unknown as object);
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
        <Input id="name" {...form.register("name")} placeholder="z. B. Spaghetti Bolognese" />
        <FormMessage className="text-left">{form.formState.errors.name?.message as string}</FormMessage>
      </section>

      <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
        <section className="grid gap-3">
          <FormLabel className="text-left" htmlFor="portions" invalid={!!form.formState.errors.portions}>Portionen</FormLabel>
          <Input id="portions" type="number" min={1} max={50} {...form.register("portions", { valueAsNumber: true })} />
          <FormMessage className="text-left">{form.formState.errors.portions?.message as string}</FormMessage>
        </section>
        <section className="grid gap-3">
          <FormLabel className="text-left" htmlFor="calories">Kalorien (optional)</FormLabel>
          <Input id="calories" type="number" min={0} max={5000} placeholder="kcal pro Portion" {...form.register("calories", { valueAsNumber: true })} />
          <FormMessage className="text-left">{form.formState.errors.calories?.message as string}</FormMessage>
        </section>
      </div>

      <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
        <section className="grid gap-2">
          <FormLabel className="text-left">Bewertung (optional)</FormLabel>
          <RatingPicker value={form.watch("rating") ?? 0} onChange={(v) => form.setValue("rating", v)} />
        </section>
        <section className="grid gap-3">
          <FormLabel className="text-left" htmlFor="tags">Tags</FormLabel>
          <TagBoxWithCreate
            options={tagOptions}
            values={form.watch("tagIds") ?? []}
            onChange={(vals) => form.setValue("tagIds", vals)}
            onCreate={onCreateTag}
            placeholder="Tag auswählen oder erstellen"
          />
        </section>
      </div>

      <section className="grid gap-3">
        <FormLabel className="text-left" htmlFor="sideDishes">Beilagen (optional)</FormLabel>
        <TagBoxWithCreate
          options={sideDishOptions}
          values={form.watch("sideDishIds") ?? []}
          onChange={(vals) => form.setValue("sideDishIds", vals)}
          onCreate={onCreateSideDish}
          placeholder="Beilage auswählen oder erstellen"
        />
      </section>

      <section className="grid gap-3">
        <FormLabel className="text-left">Bilder</FormLabel>
        <div className="flex items-center gap-3">
          <label className="inline-flex items-center gap-2 px-3 py-2 border rounded cursor-pointer">
            <ImagePlus className="h-4 w-4" />
            <span>Bild hochladen</span>
            <input className="hidden" type="file" accept="image/*" multiple onChange={e => onFilesChange(e.target.files)} />
          </label>
        </div>
        {previews.length > 0 && (
          <div className="grid grid-cols-3 gap-2">
            {previews.map((src, i) => (
              <div key={i} className="relative group">
                <img src={src} alt={`preview-${i}`} className="aspect-video w-full object-cover rounded border" />
                <button
                  type="button"
                  className="absolute top-1 right-1 hidden group-hover:block bg-red-600 text-white text-xs px-2 py-1 rounded"
                  onClick={() => removeImageAt(i)}
                  aria-label="Bild entfernen"
                >
                  Entfernen
                </button>
              </div>
            ))}
          </div>
        )}
      </section>

      {/* Parts with Ingredients */}
      <section className="grid gap-3">
        <div className="flex items-center justify-between">
          <FormLabel className="text-left">Komponenten (optional)</FormLabel>
          <Button type="button" variant="outline" onClick={() => partsArray.append({ name: "", ingredients: [{ ingredientId: "", amount: 1, unit: (NaN as unknown as number) }] })}>
            <Plus className="h-4 w-4 mr-2" /> Komponente hinzufügen
          </Button>
        </div>
        <div className="flex flex-col gap-3">
          {partsArray.fields.map((field, pIdx) => (
            <PartEditor key={field.id} pIdx={pIdx} onRemove={() => partsArray.remove(pIdx)} />
          ))}
        </div>
      </section>

      {/* Steps */}
      <section className="grid gap-3">
        <FormLabel className="text-left" invalid={!!form.formState.errors.steps}>Schritte</FormLabel>
        <div className="flex flex-col gap-3">
          {stepsArray.fields.map((field, idx) => (
            <div key={field.id} className="border rounded p-3 space-y-2">
              <div className="flex gap-1 justify-end">
                <Button type="button" variant="ghost" size="icon" onClick={() => moveStep(idx, -1)} disabled={idx === 0}>
                  <ArrowUp className="h-4 w-4" />
                </Button>
                <Button type="button" variant="ghost" size="icon" onClick={() => moveStep(idx, 1)} disabled={idx === stepsArray.fields.length - 1}>
                  <ArrowDown className="h-4 w-4" />
                </Button>
                <Button type="button" variant="ghost" size="icon" onClick={() => stepsArray.remove(idx)}>
                  <Trash2 className="h-4 w-4" />
                </Button>
              </div>
              <Textarea rows={4} placeholder={`Schritt ${idx + 1} beschreiben…`} {...form.register(`steps.${idx}.description` as const)} />
              <div className="grid grid-cols-1 md:grid-cols-3 gap-2">
                <div>
                  <FormLabel className="text-left">Gerät (optional)</FormLabel>
                  <SelectBoxWithCreate<string>
                    options={deviceOptions}
                    value={(form.watch(`steps.${idx}.deviceId` as const) as unknown as string) ?? null}
                    onChange={(v) => form.setValue(`steps.${idx}.deviceId` as const, v)}
                    onCreate={onCreateDevice}
                    placeholder="Gerät wählen"
                  />
                </div>
                <div>
                  <FormLabel className="text-left">Dauer (Minuten)</FormLabel>
                  <Input type="number" min={0} max={24*60} placeholder="z. B. 15"
                    {...form.register(`steps.${idx}.durationMinutes` as const, { valueAsNumber: true })} />
                </div>
                <div>
                  <FormLabel className="text-left">Temperatur (°C)</FormLabel>
                  <Input type="number" min={0} max={400} placeholder="optional"
                    {...form.register(`steps.${idx}.temperature` as const, { valueAsNumber: true })} />
                </div>
              </div>
            </div>
          ))}
          <Button type="button" variant="outline" onClick={() => stepsArray.append({ description: "", durationMinutes: null, temperature: null, deviceId: null })}>
            <Plus className="h-4 w-4 mr-2" /> Schritt hinzufügen
          </Button>
        </div>
        <FormMessage className="text-left">{form.formState.errors.steps?.message as string}</FormMessage>
      </section>

      {submitError && <div className="text-sm text-red-600">{submitError}</div>}
    </form>
  );
}

export function CreateRecipeDialog({ children, onCreated }: { children: React.ReactNode; onCreated?: () => void }) {
  const [open, setOpen] = React.useState(false);

  function handleSaved() {
    setOpen(false);
    onCreated?.();
    window.location.reload();
  }

  return (
    <Popup open={open} onOpenChange={setOpen}>
      <PopupTrigger asChild>{children}</PopupTrigger>
      <PopupContent className="sm:max-w-[880px] p-0 flex flex-col h-[85vh]">
        <PopupHeader className="px-6 pt-6">
          <PopupTitle>Neues Rezept</PopupTitle>
          <PopupDescription>Erstelle ein neues Rezept und speichere es.</PopupDescription>
        </PopupHeader>
        <ScrollArea className="flex-1 min-h-0 px-6 pb-6">
          <CreateRecipeForm onSaved={handleSaved} />
          <PopupFooter className="px-6 pb-6">
            <PopupClose asChild>
              <Button variant="outline">Schließen</Button>
            </PopupClose>
            <Button type="submit" form="create-recipe-form">Speichern</Button>
          </PopupFooter>
        </ScrollArea>
      </PopupContent>
    </Popup>
  );
}
