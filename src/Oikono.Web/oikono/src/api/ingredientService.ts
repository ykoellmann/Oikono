import { createSimpleLookupService } from "@/api/genericApiService";

export const IngredientApi = createSimpleLookupService("/ingredient");

export const IngredientService = {
  async listLookups() { return await IngredientApi.list(); },
  async createLookup(name: string) { return await IngredientApi.create({ name }); },
};
