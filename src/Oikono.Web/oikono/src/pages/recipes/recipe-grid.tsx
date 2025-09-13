import * as React from "react"
import {
    Pagination,
    PaginationContent,
    PaginationItem,
    PaginationLink,
    PaginationNext,
    PaginationPrevious,
} from "@/components/ui/pagination"
import type {Recipe} from "@/pages/recipes/lib/recipe.ts";
import {RecipeCard} from "@/pages/recipes/recipe-card.tsx";
import {useSearchParams} from "react-router-dom";

type RecipesGridProps = {
    recipes: Recipe[]
    page: number
    pageSize: number
    total: number
}
export function RecipesGrid({ recipes, page, pageSize, total }: RecipesGridProps) {
    const [searchParams, setSearchParams] = useSearchParams();

    const rawPage = Number.parseInt(searchParams.get("page") ?? String(page), 10) || page;

    const totalPages = Math.ceil(total / Math.max(1, pageSize));
    const maxPage = Math.max(1, totalPages); // vermeidet 0, wenn keine Rezepte

    const currentPage = Math.min(Math.max(1, rawPage), maxPage);

    React.useEffect(() => {
        if (!searchParams.get("page")) {
            setSearchParams(prev => {
                const next = new URLSearchParams(prev);
                next.set("page", "1");
                return next;
            }, { replace: true });
        }
    }, [searchParams, setSearchParams]);

    const goToPage = (p: number) => {
        const nextPage = Math.min(Math.max(1, p), maxPage);
        setSearchParams(prev => {
            const next = new URLSearchParams(prev);
            next.set("page", String(nextPage));
            return next;
        });
    };

    const paginatedRecipes = recipes ?? [];


    return (
        <div className="space-y-6">
            <div className="grid gap-6 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4">
                {paginatedRecipes.map((recipe) => (
                    <RecipeCard key={recipe.id} recipe={recipe} />
                ))}
            </div>

            {totalPages > 1 && (
                <Pagination>
                    <PaginationContent>
                        <PaginationItem>
                            <PaginationPrevious
                                href="#"
                                onClick={(e) => {
                                    e.preventDefault()
                                    goToPage(Math.max(1, currentPage - 1))
                                }}
                            />
                        </PaginationItem>

                        {Array.from({ length: totalPages }).map((_, i) => (
                            <PaginationItem key={`page-${i + 1}`}>
                                <PaginationLink
                                    href="#"
                                    isActive={currentPage === i + 1}
                                    onClick={(e) => {
                                        e.preventDefault()
                                        goToPage(i + 1)
                                    }}
                                >
                                    {i + 1}
                                </PaginationLink>
                            </PaginationItem>
                        ))}

                        <PaginationItem>
                            <PaginationNext
                                href="#"
                                onClick={(e) => {
                                    e.preventDefault()
                                    goToPage(Math.min(totalPages, currentPage + 1))
                                }}
                            />
                        </PaginationItem>
                    </PaginationContent>
                </Pagination>
            )}
        </div>
    )
}