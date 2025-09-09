import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"
import { Badge } from "@/components/ui/badge"
import type {Recipe} from "@/pages/recipes/lib/recipe.ts";

type RecipeCardProps = {
    recipe: Recipe
}

export function RecipeCard({ recipe }: RecipeCardProps) {
    return (
        <Card className="overflow-hidden">
            <CardHeader>
                <CardTitle>{recipe.name}</CardTitle>
            </CardHeader>
            <CardContent className="space-y-2">
                {/* Erstes Bild */}
                {recipe.images[0] && (
                    <img
                        src={recipe.images[0]}
                        alt={recipe.name}
                        className="w-full h-40 object-cover rounded-md"
                    />
                )}

                {/* Tags */}
                <div className="flex flex-wrap gap-2">
                    {recipe.tags.map((tag) => (
                        <Badge key={tag} variant="secondary">
                            {tag}
                        </Badge>
                    ))}
                </div>

                {/* Portionen, Kalorien, Beilagen */}
                <div className="text-sm text-muted-foreground space-y-1">
                    <p>Portionen: {recipe.portions}</p>
                    {recipe.calories && <p>Kalorien: {recipe.calories} kcal</p>}
                    {recipe.sideDishes && recipe.sideDishes.length > 0 && (
                        <p>Beilagen: {recipe.sideDishes.join(", ")}</p>
                    )}
                </div>
            </CardContent>
        </Card>
    )
}