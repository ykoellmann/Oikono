import { RecipesGrid } from "@/pages/recipes/recipe-grid.tsx"
import type { Recipe } from "@/pages/recipes/lib/recipe.ts"

const sampleRecipes: Recipe[] = [
  {
    id: "1",
    name: "Spaghetti Bolognese",
    images: ["https://picsum.photos/seed/bolo/400/240"],
    ingredients: [],
    steps: [],
    tags: ["pasta", "klassiker"],
    portions: 2,
  },
  {
    id: "2",
    name: "Gemüsepfanne",
    images: ["https://picsum.photos/seed/veggie/400/240"],
    ingredients: [],
    steps: [],
    tags: ["veggie", "schnell"],
    portions: 2,
  },
]

export default function RecipesPage() {
  return (
    <div className="p-0">
      <h1 className="text-2xl font-semibold mb-2">Rezepte</h1>
      <RecipesGrid recipes={sampleRecipes} />
    </div>
  )
}
