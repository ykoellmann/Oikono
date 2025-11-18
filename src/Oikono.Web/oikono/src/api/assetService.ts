import api from "@/api/apiService";

export const AssetService = {
  async uploadRecipeAsset(file: File, recipeId: string): Promise<string> {
    const fd = new FormData();
    fd.append("file", file, file.name);
    fd.append("recipeId", recipeId);
    const res = await api.post<{ id: string }>("/assets/recipe", fd, { headers: { "Content-Type": "multipart/form-data" } });
    return res.id;
  },
  async uploadManyForRecipe(files: File[], recipeId: string): Promise<string[]> {
    const uploads = files.map((f) => this.uploadRecipeAsset(f, recipeId));
    return await Promise.all(uploads);
  },
  getDownloadUrl(id: string): string {
    const base = (import.meta.env.VITE_API_BASE_URL as string) || "/api";
    return `${base}/assets/${encodeURIComponent(id)}`;
  },
};