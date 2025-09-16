import { createSimpleLookupService } from "@/api/genericApiService";

// Generic SideDish lookup service. Adjust base path if backend differs.
export const SideDishApi = createSimpleLookupService("/sideDish");

export const SideDishService = {
  async listLookups() { return await SideDishApi.list(); },
  async createLookup(name: string) { return await SideDishApi.create({ name }); },
};
