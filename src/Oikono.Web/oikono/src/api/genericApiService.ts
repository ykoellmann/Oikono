import api from "@/api/apiService";

// Generic CRUD service for backend generic controllers (e.g., Tag, SideDish, ...)
// Allows overriding path segments and request/response mapping per entity type.

export type IdLike = string | number;

export type ListParams = Record<string, string | number | boolean | undefined | null>;

export type Mapper<TReq, TRes, TCreate = TReq, TUpdate = Partial<TReq>> = {
  // map domain response (DTO from backend) -> app model
  fromResponse?: (dto: TRes) => TReq;
  // map app model -> backend create DTO
  toCreateDto?: (model: TCreate) => unknown;
  // map app model -> backend update DTO
  toUpdateDto?: (model: TUpdate) => unknown;
};

export class GenericApiService<TReq, TRes, TCreate = TReq, TUpdate = Partial<TReq>> {
  constructor(private readonly basePath: string, private readonly mapper: Mapper<TReq, TRes, TCreate, TUpdate> = {}) {}

  protected buildQuery(params?: ListParams): string {
    if (!params) return "";
    const usp = new URLSearchParams();
    Object.entries(params).forEach(([k, v]) => {
      if (v === undefined || v === null || v === "") return;
      usp.set(k, String(v));
    });
    const s = usp.toString();
    return s ? `?${s}` : "";
  }

  async list(params?: ListParams): Promise<TReq[]> {
    const qp = this.buildQuery(params);
    const res = await api.get<TRes[]>(`${this.basePath}${qp}`);
    return this.mapper.fromResponse ? res.map(this.mapper.fromResponse) : (res as unknown as TReq[]);
  }

  async getById(id: IdLike): Promise<TReq> {
    const res = await api.get<TRes>(`${this.basePath}/${encodeURIComponent(String(id))}`);
    return this.mapper.fromResponse ? this.mapper.fromResponse(res) : (res as unknown as TReq);
  }

  async create(payload: TCreate): Promise<TReq> {
    const body = this.mapper.toCreateDto ? this.mapper.toCreateDto(payload) : (payload as unknown);
    const res = await api.post<TRes>(this.basePath, body);
    return this.mapper.fromResponse ? this.mapper.fromResponse(res) : (res as unknown as TReq);
  }

  async update(id: IdLike, payload: TUpdate): Promise<TReq> {
    const body = this.mapper.toUpdateDto ? this.mapper.toUpdateDto(payload) : (payload as unknown);
    const res = await api.put<TRes>(`${this.basePath}/${encodeURIComponent(String(id))}`, body);
    return this.mapper.fromResponse ? this.mapper.fromResponse(res) : (res as unknown as TReq);
  }

  async delete(id: IdLike): Promise<void> {
    await api.delete<void>(`${this.basePath}/${encodeURIComponent(String(id))}`);
  }
}

// Convenience factory for simple string valued lookups like Tag, SideDish, etc. with Id + Name
// Backend example response: { id: Guid, name: string }
export type SimpleLookup = { id: string; name: string };
export type SimpleLookupCreate = { name: string };

export function createSimpleLookupService(path: string) {
  return new GenericApiService<SimpleLookup, SimpleLookup, SimpleLookupCreate, Partial<SimpleLookupCreate>>(path, {
    fromResponse: (dto) => dto,
    toCreateDto: (m) => m,
    toUpdateDto: (m) => m,
  });
}

// Specific instance for Tags using backend generic TagController at /api/tag
export const TagApi = createSimpleLookupService("/tag");
