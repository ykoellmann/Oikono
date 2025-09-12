import {Card, CardContent, CardHeader, CardTitle} from "@/components/ui/card.tsx";
import {Separator} from "@/components/ui/separator.tsx";
import type {IngredientPart} from "@/pages/recipes/lib/recipe.ts";


export function IngredientPartCard(part: IngredientPart) {
    return (
        <Card>
            {part.name !== null ? <CardHeader><CardTitle>{part.name}</CardTitle></CardHeader> : <span></span>}
            <CardContent>
                <Separator className="my-2"/>
                {part.ingredients.map((ing) => (
                    <div>
                        <div>{ing.amount} {ing.unit} {ing.name}</div>
                        <Separator className="my-2"/>
                    </div>
                ))}
            </CardContent>
        </Card>
    )
}