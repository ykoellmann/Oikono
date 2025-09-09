import * as React from "react"
import {
    Pagination,
    PaginationContent,
    PaginationEllipsis,
    PaginationItem,
    PaginationLink,
    PaginationNext,
    PaginationPrevious,
} from "@/components/ui/pagination"
import type {Recipe} from "@/pages/recipes/lib/recipe.ts";
import {RecipeCard} from "@/pages/recipes/recipe-card.tsx";

type RecipesGridProps = {
    recipes: Recipe[]
    itemsPerPage?: number
}

export function RecipesGrid({ recipes, itemsPerPage = 8 }: RecipesGridProps) {
    const [page, setPage] = React.useState(1)

    const totalPages = Math.ceil(recipes.length / itemsPerPage)

    const paginatedRecipes = React.useMemo(() => {
        const start = (page - 1) * itemsPerPage
        return recipes.slice(start, start + itemsPerPage)
    }, [page, recipes, itemsPerPage])

    return (
        <div className="space-y-6">
            {/* Grid */}
            <div className="grid gap-6 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4">
                {paginatedRecipes.map((recipe) => (
                    <RecipeCard key={recipe.id} recipe={recipe} />
                ))}
            </div>

            {/* Pagination */}
            {totalPages > 1 && (
                <Pagination>
                    <PaginationContent>
                        <PaginationItem>
                            <PaginationPrevious
                                href="#"
                                onClick={(e) => {
                                    e.preventDefault()
                                    setPage((p) => Math.max(1, p - 1))
                                }}
                            />
                        </PaginationItem>

                        {Array.from({ length: totalPages }).map((_, i) => (
                            <PaginationItem key={i}>
                                <PaginationLink
                                    href="#"
                                    isActive={page === i + 1}
                                    onClick={(e) => {
                                        e.preventDefault()
                                        setPage(i + 1)
                                    }}
                                >
                                    {i + 1}
                                </PaginationLink>
                            </PaginationItem>
                        ))}

                        {totalPages > 5 && <PaginationEllipsis />}

                        <PaginationItem>
                            <PaginationNext
                                href="#"
                                onClick={(e) => {
                                    e.preventDefault()
                                    setPage((p) => Math.min(totalPages, p + 1))
                                }}
                            />
                        </PaginationItem>
                    </PaginationContent>
                </Pagination>
            )}
        </div>
    )
}
