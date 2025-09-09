export type Ingredient = {
    name: string
    amount: number
    unit: string
}

export type RecipeStep = {
    step: string
    duration?: number         // Minuten
    device?: "oven" | "stove" | "mixer" | string
    temperature?: number      // °C, falls relevant
}

export type Recipe = {
    id: string
    name: string
    images: string[]          // mehrere Bilder
    ingredients: Ingredient[]
    steps: RecipeStep[]
    tags: string[]            // z. B. ["vegan", "schnell"]

    portions: number          // Pflichtfeld
    calories?: number         // optional, kcal pro Portion
    sideDishes?: string[]     // optional, z. B. ["Reis", "Salat"]
}