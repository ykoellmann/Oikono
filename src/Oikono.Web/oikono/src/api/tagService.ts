import { TagApi, type SimpleLookup } from "@/api/genericApiService";

// Dedicated Tag service built on the generic API
// Provides both lookup-based and convenience string-based operations

export const TagService = {
  // Full lookup objects
  async listLookups(): Promise<SimpleLookup[]> {
    return await TagApi.list();
  },
  async getById(id: string): Promise<SimpleLookup> {
    return await TagApi.getById(id);
  },
  async createLookup(name: string): Promise<SimpleLookup> {
    return await TagApi.create({ name });
  },
  async update(id: string, name: string): Promise<SimpleLookup> {
    return await TagApi.update(id, { name });
  },
  async delete(id: string): Promise<void> {
    await TagApi.delete(id);
  },

  // Convenience: strings only (names)
  async list(): Promise<string[]> {
    const items = await TagApi.list();
    return items.map(i => i.name);
  },
  async create(name: string): Promise<void> {
    const v = (name || "").trim();
    if (!v) return;
    await TagApi.create({ name: v });
  },
};
