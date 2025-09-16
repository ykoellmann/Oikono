import React from "react";
import { CheckIcon, PlusIcon } from "lucide-react";
import { RecipeService } from "@/pages/recipes/lib/recipeService";
import { TagService } from "@/api/tagService";
import { cn } from "@/lib/utils";

// Fallback minimal UI if shadcn tags are not present in the project.
// This component emulates the provided API with simple markup and Tailwind.

export type TagsEditorProps = {
  recipeId: string;
  value: string[];
  onChange?: (tags: string[]) => void;
  className?: string;
};

export default function TagsEditor({ recipeId, value, onChange, className }: TagsEditorProps) {
  const [options, setOptions] = React.useState<string[]>([]);
  const [selected, setSelected] = React.useState<string[]>(value ?? []);
  const [query, setQuery] = React.useState("");
  const [loading, setLoading] = React.useState(true);
  const [saving, setSaving] = React.useState(false);

  React.useEffect(() => {
    setSelected(value ?? []);
  }, [value]);

  React.useEffect(() => {
    let active = true;
    setLoading(true);
    TagService.list()
      .then((tags) => { if (active) setOptions(tags); })
      .finally(() => { if (active) setLoading(false); });
    return () => { active = false; };
  }, []);

  const persist = React.useMemo(() => {
    let t: number | undefined;
    return (tags: string[]) => {
      setSaving(true);
      window.clearTimeout(t);
      t = window.setTimeout(async () => {
        try {
          await RecipeService.update(recipeId, { tags });
        } finally {
          setSaving(false);
        }
      }, 400);
    };
  }, [recipeId]);

  const setSelectedAndPersist = (next: string[]) => {
    setSelected(next);
    onChange?.(next);
    persist(next);
  };

  const handleRemove = (tag: string) => {
    if (!selected.includes(tag)) return;
    setSelectedAndPersist(selected.filter((t) => t !== tag));
  };

  const handleToggle = (tag: string) => {
    if (selected.includes(tag)) {
      handleRemove(tag);
    } else {
      setSelectedAndPersist([...selected, tag]);
    }
  };

  const handleCreate = async (label?: string) => {
    const v = (label ?? query).trim();
    if (!v) return;
    if (!options.includes(v)) {
      try {
        await TagService.create(v);
        setOptions((prev) => [...prev, v]);
      } catch {
        // ignore
      }
    }
    if (!selected.includes(v)) {
      setSelectedAndPersist([...selected, v]);
    }
    setQuery("");
  };

  const filtered = React.useMemo(() => {
    const q = query.toLowerCase().trim();
    return q ? options.filter((o) => o.toLowerCase().includes(q)) : options;
  }, [options, query]);

  return (
    <div className={cn("w-full", className)}>
      {/* Trigger/values */}
      <div className="flex flex-wrap gap-2 rounded-md border bg-card px-2 py-1">
        {selected.length === 0 && (
          <span className="text-xs text-muted-foreground">Keine Tags ausgewählt</span>
        )}
        {selected.map((t) => (
          <span key={t} className="flex items-center gap-1 rounded bg-secondary px-2 py-0.5 text-xs">
            {t}
            <button onClick={() => handleRemove(t)} className="ml-1 text-muted-foreground hover:text-foreground">×</button>
          </span>
        ))}
      </div>

      {/* Content */}
      <div className="mt-2 rounded-md border">
        <div className="flex items-center gap-2 p-2">
          <input
            value={query}
            onChange={(e) => setQuery(e.target.value)}
            onKeyDown={(e) => {
              if (e.key === "Enter") {
                e.preventDefault();
                handleCreate();
              }
            }}
            placeholder="Tag suchen oder neu erstellen…"
            className="w-full bg-transparent text-sm outline-none"
          />
          <button
            type="button"
            onClick={() => handleCreate()}
            className="inline-flex items-center gap-1 rounded border px-2 py-1 text-xs hover:bg-accent"
          >
            <PlusIcon size={14} />
            Erstellen
          </button>
        </div>
        <div className="max-h-44 overflow-auto border-t p-2">
          {loading ? (
            <div className="text-xs text-muted-foreground">Lade Tags…</div>
          ) : filtered.length === 0 ? (
            <div className="text-xs text-muted-foreground">
              Keine Treffer. Drücke Enter um „{query}“ zu erstellen.
            </div>
          ) : (
            <div className="grid grid-cols-1 gap-1">
              {filtered.map((tag) => {
                const isSel = selected.includes(tag);
                return (
                  <button
                    key={tag}
                    type="button"
                    onClick={() => handleToggle(tag)}
                    className={cn(
                      "flex items-center justify-between rounded px-2 py-1 text-left text-sm hover:bg-accent",
                      isSel && "bg-accent"
                    )}
                  >
                    <span>{tag}</span>
                    {isSel && <CheckIcon size={14} className="text-muted-foreground" />}
                  </button>
                );
              })}
            </div>
          )}
        </div>
        {saving && (
          <div className="border-t p-2 text-right text-[11px] text-muted-foreground">Speichere…</div>
        )}
      </div>
    </div>
  );
}
