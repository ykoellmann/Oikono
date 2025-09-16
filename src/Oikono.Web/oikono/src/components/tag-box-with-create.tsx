import * as React from "react";
import {X, Plus} from "lucide-react";
import {
    Command,
    CommandGroup,
    CommandItem,
    CommandList,
    CommandInput,
} from "@/components/ui/command";
import {Popover, PopoverContent, PopoverTrigger} from "@/components/ui/popover";
import {Button} from "@/components/ui/button";

type Option = {
    label: string;
    value: string;
};

interface TagBoxWithCreateProps {
    options: Option[];
    values: string[];
    onChange: (values: string[]) => void;
    onCreate: (value: string) => void;
    placeholder?: string;
}

export function TagBoxWithCreate({
                                     options,
                                     values,
                                     onChange,
                                     onCreate,
                                     placeholder = "Select or create...",
                                 }: TagBoxWithCreateProps) {
    const [open, setOpen] = React.useState(false);
    const [search, setSearch] = React.useState("");

    const filtered = options.filter(
        o => o.label.toLowerCase().includes(search.toLowerCase()) && !values.includes(o.value)
    );

    const remove = (val: string) => onChange(values.filter(v => v !== val));

    return (
        <div className="flex flex-wrap gap-2">
            {values.map(val => {
                const label = options.find(o => o.value === val)?.label ?? val;
                return (
                    <span key={val} className="flex items-center gap-1 rounded-full bg-muted px-2 py-1 text-sm">
            {label}
                        <button onClick={() => remove(val)}>
              <X className="h-3 w-3" />
            </button>
          </span>
                );
            })}

            <Popover open={open} onOpenChange={setOpen}>
                <PopoverTrigger asChild>
                    <Button variant="outline" size="sm">+ {placeholder}</Button>
                </PopoverTrigger>
                <PopoverContent className="p-0 w-[200px]">
                    <Command shouldFilter={false}>
                        <CommandInput
                            placeholder="Search..."
                            value={search}
                            onValueChange={setSearch}
                        />
                        <CommandList>
                            {filtered.length > 0 && (
                                <CommandGroup>
                                    {filtered.map(o => (
                                        <CommandItem
                                            key={o.value}
                                            onSelect={() => {
                                                onChange([...values, o.value]);
                                                setOpen(false);
                                                setSearch("");
                                            }}
                                        >
                                            {o.label}
                                        </CommandItem>
                                    ))}
                                </CommandGroup>
                            )}

                            {filtered.length === 0 && search.length > 0 && (
                                <CommandGroup>
                                    <CommandItem
                                        key="__create__"
                                        onSelect={() => {
                                            // nur Option erstellen
                                            onCreate(search);
                                            setOpen(false);
                                            setSearch("");
                                        }}
                                    >
                                        <Plus className="mr-2 h-4 w-4" />
                                        Create "{search}"
                                    </CommandItem>
                                </CommandGroup>
                            )}
                        </CommandList>
                    </Command>
                </PopoverContent>
            </Popover>
        </div>
    );
}
