export type Ingredient = {
    name: string
    amount: number
    unit: string
}

/**
 * Merge a list of ingredients by summing amounts for identical name+unit pairs.
 * - Case-insensitive, trims whitespace
 * - Only sums if unit string matches (after trim, case-insensitive)
 */
export function mergeAndSumIngredients(ings: Ingredient[]): Ingredient[] {
    const map = new Map<string, Ingredient>();
    for (const ing of ings ?? []) {
        const name = (ing.name ?? "").trim();
        const unit = (ing.unit ?? "").trim();
        const key = `${name.toLowerCase()}__${unit.toLowerCase()}`;
        const prev = map.get(key);
        if (prev) {
            prev.amount += ing.amount;
        } else {
            map.set(key, { name, unit, amount: ing.amount });
        }
    }
    return Array.from(map.values());
}

export type RecipeStep = {
    step: string
    duration?: number         // Minuten
    device?: "oven" | "stove" | "mixer" | string
    temperature?: number      // °C, falls relevant
}

export type IngredientPart = {
    name: string | null
    ingredients: Ingredient[]
}

export type Recipe = {
    id: string
    name: string
    images: string[]          // mehrere Bilder
    ingredients: Ingredient[]
    parts?: IngredientPart[]  // optionale Gruppen von Zutaten (z. B. Teig, Füllung, Crumble)
    steps: RecipeStep[]
    tags: string[]            // z. B. ["vegan", "schnell"]

    portions: number          // Pflichtfeld
    calories?: number         // optional, kcal pro Portion
    sideDishes?: string[]     // optional, z. B. ["Reis", "Salat"]
    rating?: number           // ganzzahlig 0-5
}