import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"
import { Badge } from "@/components/ui/badge"
import type {Recipe, Asset} from "@/pages/recipes/lib/recipe.ts";
import { Separator } from "@/components/ui/separator";
import {Link} from "react-router-dom";
import placeholder from "@/assets/placeholder.png";

function getAssetDataUrl(asset: Asset): string {
    const ct = asset?.contentType || "image/jpeg";
    const data = asset?.data || "";
    return `data:${ct};base64,${data}`;
}

type RecipeCardProps = {
    recipe: Recipe
}

export function RecipeCard({ recipe }: RecipeCardProps) {
    return (
        <Link to={`/recipes/${recipe.id}`} className="block">
        <Card className="overflow-hidden hover:shadow-md transition-shadow">
            <CardHeader>
                <CardTitle>{recipe.name}</CardTitle>
            </CardHeader>
            <CardContent className="space-y-2">
                {/* Erstes Bild mit Fallback */}
                <img
                    src={recipe.images && recipe.images.length > 0 ? getAssetDataUrl(recipe.images[0]) : placeholder}
                    alt={recipe.name}
                    className="w-full h-40 object-cover rounded-md"
                />

                {/* Tags */}
                <div className="flex flex-wrap gap-2 h-16 overflow-hidden">
                    {recipe.tags.slice(0, 5).map((tag) => (
                        <Badge className="max-h-6" key={tag.id} variant="secondary">
                            {tag.name}
                        </Badge>
                    ))}
                    {recipe.tags.length > 5 && (
                        <Badge variant="outline">+{recipe.tags.length - 5}</Badge>
                    )}
                </div>

                <Separator className="my-4" />

                {/* Portionen, Kalorien, Beilagen */}
                <div className="text-sm text-muted-foreground space-x-2">
                    <div className="flex justify-between text-sm text-muted-foreground">
                        <span>Portionen: {recipe.portions}</span>
                        {recipe.calories && (
                            <span>Kalorien: {recipe.calories} kcal</span>
                        )}
                    </div>
                </div>
            </CardContent>
        </Card>
        </Link>
    )
}