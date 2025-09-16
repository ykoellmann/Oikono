import api from "@/api/apiService";
import type { Option } from "@/components/select-box-with-create";

export const UnitService = {
  // Fetch from RecipeController /units which returns [{label: string, value: number}]
  async listOptions(): Promise<Option[]> {
    const res = await api.get<Array<{ label: string; value: number }>>("/recipes/units");
    // Map directly to Option (value can be number now)
    return res.map(u => ({ label: u.label, value: u.value }));
  },
};
