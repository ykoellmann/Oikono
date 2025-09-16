import api from "@/api/apiService";
import type { Recipe } from "@/pages/recipes/lib/recipe";

export type SortDirection = "asc" | "desc";

export type RecipeSortField =
  | "name"
  | "createdAt"
  | "updatedAt"
  | "rating"
  | "calories"
  | "portions";

export type RecipeQuery = {
  page?: number; // 1-based
  pageSize?: number; // default 12
  search?: string; // free text over name, tags, ingredients, etc.
  sortBy?: RecipeSortField;
  sortDir?: SortDirection;
  // Dynamic filters (all optional, arrays imply OR within field, AND across fields)
  tags?: string[];
  sideDishes?: string[];
  devices?: string[]; // from steps.device
  ingredients?: string[]; // ingredient name contains
  minRating?: number;
  maxCalories?: number;
};

export type PagedResult<T> = {
  items: T[];
  total: number; // total items matching the query
  page: number; // 1-based
  pageSize: number;
};

function buildQueryParams(q: RecipeQuery): string {
  const params = new URLSearchParams();
  if (q.page) params.set("page", String(q.page));
  if (q.pageSize) params.set("pageSize", String(q.pageSize));
  if (q.search) params.set("search", q.search);
  if (q.sortBy) params.set("sortBy", q.sortBy);
  if (q.sortDir) params.set("sortDir", q.sortDir);
  if (q.tags && q.tags.length) params.set("tags", q.tags.join(","));
  if (q.sideDishes && q.sideDishes.length) params.set("sideDishes", q.sideDishes.join(","));
  if (q.devices && q.devices.length) params.set("devices", q.devices.join(","));
  if (q.ingredients && q.ingredients.length) params.set("ingredients", q.ingredients.join(","));
  if (q.minRating != null) params.set("minRating", String(q.minRating));
  if (q.maxCalories != null) params.set("maxCalories", String(q.maxCalories));
  const s = params.toString();
  return s ? `?${s}` : "";
}

// REST endpoints – adjust paths to your backend as needed
const BASE = "/recipes";

export const RecipeService = {
  async list(q: RecipeQuery = {}): Promise<PagedResult<Recipe>> {
    const qp = buildQueryParams(q);
    return await api.get<PagedResult<Recipe>>(`${BASE}${qp}`);
  },

  async getById(id: string): Promise<Recipe> {
    return await api.get<Recipe>(`${BASE}/${encodeURIComponent(id)}`);
  },

  async create(payload: unknown): Promise<Recipe> {
    if (payload instanceof FormData) {
      const res = await api.post<Recipe>(BASE, payload, { headers: { "Content-Type": "multipart/form-data" } });
      return res;
    }
    return await api.post<Recipe>(BASE, payload);
  },

  async update(id: string, payload: Partial<Recipe>): Promise<Recipe> {
    return await api.put<Recipe>(`${BASE}/${encodeURIComponent(id)}`, payload);
  },

  async delete(id: string): Promise<void> {
    await api.delete<void>(`${BASE}/${encodeURIComponent(id)}`);
  },
};

export type { RecipeQuery as RecipesQuery, PagedResult as RecipesPagedResult };
