import * as React from "react";
import { RecipesGrid } from "@/pages/recipes/recipe-grid.tsx";
import PageLayout from "@/components/page-layout.tsx";
import { RecipeService } from "@/pages/recipes/lib/recipeService";
import { useSearchParams } from "react-router-dom";
import type { Recipe } from "@/pages/recipes/lib/recipe";
import {CirclePlus} from "lucide-react";

export default function RecipesPage() {
  const [searchParams] = useSearchParams();
  const page = Number.parseInt(searchParams.get("page") ?? "1", 10) || 1;
  const pageSize = 8; // default page size for grid

  const [state, setState] = React.useState<{ items: Recipe[]; total: number; loading: boolean; error: string | null }>({
    items: [],
    total: 0,
    loading: true,
    error: null,
  });

  React.useEffect(() => {
    let isActive = true;
    setState((s) => ({ ...s, loading: true, error: null }));
    RecipeService.list({ page, pageSize })
      .then((res) => {
        if (!isActive) return;
        setState({ items: res.items, total: res.total, loading: false, error: null });
      })
      .catch((err) => {
        if (!isActive) return;
        setState((s) => ({ ...s, loading: false, error: err?.message ?? "Fehler beim Laden" }));
      });
    return () => { isActive = false; };
  }, [page]);

  return (
    <PageLayout title="Rezepte" >
      <div className="flex">
        <CirclePlus></CirclePlus>
      </div>
      {state.loading && <div className="text-sm text-muted-foreground">Lade Rezepte…</div>}
      {state.error && !state.loading && (
        <div className="text-sm text-red-600">{state.error}</div>
      )}
      {!state.loading && !state.error && (
        <RecipesGrid
          recipes={state.items}
          page={page}
          pageSize={pageSize}
          total={state.total}
        />
      )}
    </PageLayout>
  );
}
