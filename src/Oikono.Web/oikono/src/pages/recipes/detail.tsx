import {useEffect, useState} from "react";
import {useNavigate, useParams} from "react-router-dom";
import type {Recipe} from "@/pages/recipes/lib/recipe.ts";
import {mergeAndSumIngredients} from "@/pages/recipes/lib/recipe.ts";
import PageLayout from "@/components/page-layout.tsx";
import {Separator} from "@/components/ui/separator";
import {Sheet, SheetContent, SheetHeader, SheetTitle, SheetTrigger} from "@/components/ui/sheet";
import {Button} from "@/components/ui/button";
import {Collapsible, CollapsibleContent, CollapsibleTrigger} from "@/components/ui/collapsible";
import {Utensils} from "lucide-react";
import {Tabs, TabsList, TabsTrigger, TabsContent} from "@/components/ui/tabs";
import TagsEditor from "@/components/tags-editor";
import {
    Carousel,
    CarouselContent,
    CarouselItem,
    CarouselNext,
    CarouselPrevious,
    type CarouselApi
} from "@/components/ui/carousel";
import {cn} from "@/lib/utils";
import {IngredientPartCard} from "@/pages/recipes/ingredient-part-card.tsx";

// Load recipe from API via RecipeService
import { RecipeService } from "@/pages/recipes/lib/recipeService";

export default function RecipeDetailPage() {
    const {id} = useParams();
    const navigate = useNavigate();

    const [recipe, setRecipe] = useState<Recipe | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        let active = true;
        if (!id) {
            setRecipe(null);
            setLoading(false);
            setError("Kein Rezept angegeben");
            return;
        }
        setLoading(true);
        setError(null);
        RecipeService.getById(id)
            .then(r => { if (active) { setRecipe(r); setLoading(false); }})
            .catch(err => { if (active) { setError(err?.message ?? "Fehler beim Laden"); setLoading(false); }});
        return () => { active = false; };
    }, [id]);
    const [open, setOpen] = useState(false);
    const [showSidebar, setShowSidebar] = useState(true);
    // Carousel 07 state
    const [carouselApi, setCarouselApi] = useState<CarouselApi | undefined>(undefined);
    const [currentSlide, setCurrentSlide] = useState(0);
    const [slideCount, setSlideCount] = useState(0);

    useEffect(() => {
        if (!carouselApi) return;
        setSlideCount(carouselApi.scrollSnapList().length);
        setCurrentSlide(carouselApi.selectedScrollSnap() + 1);
        const handler = () => setCurrentSlide(carouselApi.selectedScrollSnap() + 1);
        carouselApi.on("select", handler);
        return () => {
            try {
                carouselApi.off("select", handler);
            } catch {
                // ignore unbind errors on teardown
            }
        };
    }, [carouselApi]);

    if (loading) {
        return (
            <PageLayout title="Rezept wird geladen">
                <p className="text-muted-foreground">Lade…</p>
            </PageLayout>
        );
    }
    if (error || !recipe) {
        return (
            <PageLayout title="Rezept nicht gefunden">
                <div className="space-y-4">
                    <p className="text-muted-foreground">{error ?? "Das gewünschte Rezept existiert nicht."}</p>
                    <Button onClick={() => navigate(-1)}>Zurück</Button>
                </div>
            </PageLayout>
        );
    }

    // @ts-ignore
    return (
        <PageLayout title={recipe.name}>
            {/* Rating with utensils (integer only) */}
            <div className="flex items-center gap-2 text-sm mb-2">
                {Array.from({length: 5}).map((_, i) => {
                    const value = Math.max(0, Math.min(5, Math.floor(recipe.rating ?? 0)))
                    const filled = i < value
                    return (
                        <Utensils
                            key={i}
                            className={`w-4 h-4 ${filled ? 'text-amber-500' : 'text-muted-foreground/40'}`}
                        />
                    )
                })}
            </div>

            {/* Top section: image left, info right on large; stacked on small */}
            <div className="grid grid-cols-1 lg:grid-cols-3 gap-6 items-start">
                {/* Image gallery - Carousel 07 pattern */}
                {recipe.images && recipe.images.length > 0 && (
                    <div className="lg:col-span-2 w-full py-2">
                        <Carousel className="w-full" setApi={setCarouselApi}>
                            <CarouselContent>
                                {recipe.images.map((src, index) => (
                                    <CarouselItem key={index}>
                                        <div
                                            className="relative w-full overflow-hidden rounded-md h-48 sm:h-56 md:h-[50vh] lg:h-[50vh] max-h-[640px]">
                                            <img
                                                src={src}
                                                alt={`${recipe.name} ${index + 1}`}
                                                className="absolute inset-0 h-full w-full object-cover"
                                            />
                                        </div>
                                    </CarouselItem>
                                ))}
                            </CarouselContent>
                            <CarouselPrevious className="top-[calc(100%+0.5rem)] translate-y-0 left-0"/>
                            <CarouselNext className="top-[calc(100%+0.5rem)] translate-y-0 left-2 translate-x-full"/>
                        </Carousel>
                        <div className="mt-4 flex items-center justify-end gap-2">
                            {Array.from({length: slideCount}).map((_, index) => (
                                <button
                                    key={index}
                                    onClick={() => carouselApi?.scrollTo(index)}
                                    className={cn("h-3.5 w-3.5 rounded-full border-2", {
                                        "border-primary": currentSlide === index + 1,
                                    })}
                                />
                            ))}
                        </div>
                    </div>
                )}

                {/* Right: Info moved next to image */}
                <div className="lg:col-span-1 space-y-3">
                    {/* Tags Editor */}
                    <TagsEditor
                        recipeId={recipe.id}
                        value={recipe.tags}
                        onChange={(next) => setRecipe({ ...recipe, tags: next })}
                    />
                    <div className="text-sm text-muted-foreground grid grid-cols-2 gap-2">
                        <div>Portionen: <span className="font-medium text-foreground">{recipe.portions}</span></div>
                        {recipe.calories && (
                            <div>Kalorien: <span className="font-medium text-foreground">{recipe.calories} kcal</span>
                            </div>
                        )}
                        {recipe.sideDishes && recipe.sideDishes.length > 0 && (
                            <div className="col-span-2">Beilagen: {recipe.sideDishes.join(", ")}</div>
                        )}
                    </div>
                </div>
            </div>

            <Separator className="my-4"/>

            {/* Ingredients section */}
            <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
                {/* Left: Ingredients */}
                <div className="lg:col-span-2">
                    <h2 className="text-xl font-semibold mb-2">Zutaten</h2>
                    <Tabs defaultValue="gesamt">
                        <TabsList className="mb-2">
                            <TabsTrigger value="gesamt">Gesamt</TabsTrigger>
                            {(recipe.parts && recipe.parts.length > 0) && (
                                <TabsTrigger value="komponenten">Komponentenweise</TabsTrigger>
                            )}
                        </TabsList>
                        <TabsContent value="gesamt">
                            <IngredientPartCard
                                name={null}
                                ingredients={mergeAndSumIngredients((recipe.parts?.flatMap(p => p.ingredients)))}/>
                        </TabsContent>
                        <TabsContent value="komponenten">
                            <div className="space-y-3">
                                {(recipe.parts ?? []).map((p) => (
                                    <IngredientPartCard name={p.name} ingredients={p.ingredients}/>
                                ))}
                            </div>
                        </TabsContent>
                    </Tabs>
                </div>
            </div>

            <Separator className="my-4"/>

            {/* Steps with sticky quick-access to ingredients */}
            <div className="flex items-center justify-between mb-2">
                <h2 className="text-xl font-semibold">Anleitung</h2>
                {/* Desktop: collapsible on the right */}
                <div className="hidden md:block">
                    <Collapsible open={showSidebar} onOpenChange={setShowSidebar}>
                        <CollapsibleTrigger asChild>
                            <Button variant="outline" size="sm">Zutaten ein-/ausblenden</Button>
                        </CollapsibleTrigger>
                        <CollapsibleContent>
                            <div className="mt-3 p-3 border rounded-md bg-card">
                                <h3 className="font-medium mb-2">Zutaten</h3>
                                <Tabs defaultValue="gesamt">
                                    <TabsList className="mb-2">
                                        <TabsTrigger value="gesamt">Gesamt</TabsTrigger>
                                        {(recipe.parts && recipe.parts.length > 0) && (
                                            <TabsTrigger value="komponenten">Komponentenweise</TabsTrigger>
                                        )}
                                    </TabsList>
                                    <TabsContent value="gesamt">
                                        <ul className="list-disc pl-5 space-y-1">
                                            {mergeAndSumIngredients((recipe.parts?.flatMap(p => p.ingredients))).map((ing, i) => (
                                                <li key={`c-${ing.name}-${ing.unit}-${i}`}>{ing.amount} {ing.unit} {ing.name}</li>
                                            ))}
                                        </ul>
                                    </TabsContent>
                                    <TabsContent value="komponenten">
                                        <div className="space-y-3">
                                            {(recipe.parts ?? []).map((p, idx) => (
                                                <div key={idx}>
                                                    <h4 className="font-medium mb-1">{p.name}</h4>
                                                    <ul className="list-disc pl-5 space-y-1">
                                                        {p.ingredients.map((ing, i) => (
                                                            <li key={`c-${p.name}-${ing.name}-${ing.unit}-${i}`}>{ing.amount} {ing.unit} {ing.name}</li>
                                                        ))}
                                                    </ul>
                                                </div>
                                            ))}
                                        </div>
                                    </TabsContent>
                                </Tabs>
                            </div>
                        </CollapsibleContent>
                    </Collapsible>
                </div>
                {/* Mobile: sheet full screen */}
                <div className="md:hidden">
                    <Sheet open={open} onOpenChange={setOpen}>
                        <SheetTrigger asChild>
                            <Button variant="outline" size="sm">Zutaten anzeigen</Button>
                        </SheetTrigger>
                        <SheetContent side="right" className="w-[85vw] sm:w-[400px]">
                            <SheetHeader>
                                <SheetTitle>Zutaten</SheetTitle>
                            </SheetHeader>
                            <div className="mt-4 space-y-2">
                                <Tabs defaultValue="gesamt">
                                    <TabsList className="mb-2">
                                        <TabsTrigger value="gesamt">Gesamt</TabsTrigger>
                                        {(recipe.parts && recipe.parts.length > 0) && (
                                            <TabsTrigger value="komponenten">Komponentenweise</TabsTrigger>
                                        )}
                                    </TabsList>
                                    <TabsContent value="gesamt">
                                        <ul className="list-disc pl-5 space-y-1">
                                            {mergeAndSumIngredients((recipe.parts?.flatMap(p => p.ingredients))).map((ing, i) => (
                                                <li key={`m-${ing.name}-${ing.unit}-${i}`}>{ing.amount} {ing.unit} {ing.name}</li>
                                            ))}
                                        </ul>
                                    </TabsContent>
                                    <TabsContent value="komponenten">
                                        <div className="space-y-3">
                                            {(recipe.parts ?? []).map((p, idx) => (
                                                <div key={idx}>
                                                    <h4 className="font-medium mb-1">{p.name}</h4>
                                                    <ul className="list-disc pl-5 space-y-1">
                                                        {p.ingredients.map((ing, i) => (
                                                            <li key={`m-${p.name}-${ing.name}-${ing.unit}-${i}`}>{ing.amount} {ing.unit} {ing.name}</li>
                                                        ))}
                                                    </ul>
                                                </div>
                                            ))}
                                        </div>
                                    </TabsContent>
                                </Tabs>
                            </div>
                        </SheetContent>
                    </Sheet>
                </div>
            </div>

            <ol className="space-y-4">
                {recipe.steps.map((s, i) => (
                    <li key={i} className="p-4 rounded-md border bg-card">
                        <div className="flex items-start justify-between gap-4">
                            <div>
                                <div className="font-medium">Schritt {i + 1}</div>
                                <p className="text-sm mt-1">{s.step}</p>
                            </div>
                            <div className="text-xs text-muted-foreground text-right min-w-24">
                                {s.duration ? <div>Dauer: {s.duration} min</div> : null}
                                {s.device ? <div>Gerät: {s.device}</div> : null}
                                {s.temperature ? <div>Temp.: {s.temperature}°C</div> : null}
                            </div>
                        </div>
                    </li>
                ))}
            </ol>
        </PageLayout>
    );
}
