import { createSimpleLookupService } from "@/api/genericApiService";

export const DeviceApi = createSimpleLookupService("/device");

export const DeviceService = {
  async listLookups() { return await DeviceApi.list(); },
  async createLookup(name: string) { return await DeviceApi.create({ name }); },
};
