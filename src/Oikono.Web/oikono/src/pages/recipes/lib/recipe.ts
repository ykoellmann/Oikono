export type IngredientRef = {
    id: string
    name: string
}
export type PartIngredient = {
    ingredient: IngredientRef
    amount: number
    unit: string
}

export type MergedIngredient = {
    name: string
    unit: string
    amount: number
}

/**
 * Merge a list of ingredients by summing amounts for identical name+unit pairs.
 * - Case-insensitive, trims whitespace
 * - Only sums if unit string matches (after trim, case-insensitive)
 */
export function mergeAndSumIngredients(ings?: PartIngredient[] | null): MergedIngredient[] {
    const map = new Map<string, MergedIngredient>();
    for (const ing of ings ?? []) {
        const name = (ing.ingredient?.name ?? "").trim();
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

export type Device = {
    id: string
    name: string
}

export type RecipeStep = {
    description: string
    duration?: number         // Minuten
    device?: Device
    temperature?: number      // °C, falls relevant
}

export type IngredientPart = {
    name: string | null
    ingredients: PartIngredient[]
}

export type Asset = {
    id: string,
    fileName: string,
    contentType: string,
    data: string, // base64
}

export type SideDish = {
    id: string
    name: string
}

export type Tag = {
    id: string
    name: string
}

export type Recipe = {
    id: string
    name: string
    images: Asset[]          // mehrere Bilder
    parts: IngredientPart[]  // optionale Gruppen von Zutaten (z. B. Teig, Füllung, Crumble)
    steps: RecipeStep[]
    tags: Tag[]            // z. B. ["vegan", "schnell"]

    portions: number          // Pflichtfeld
    calories?: number         // optional, kcal pro Portion
    sideDishes?: SideDish[]     // optional, z. B. ["Reis", "Salat"]
    rating?: number           // ganzzahlig 0-5
}