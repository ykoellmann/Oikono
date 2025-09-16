import * as React from "react";
import {Plus, ChevronsUpDown, Check} from "lucide-react";
import {Command, CommandGroup, CommandItem, CommandList, CommandInput} from "@/components/ui/command";
import {Popover, PopoverContent, PopoverTrigger} from "@/components/ui/popover";
import {Button} from "@/components/ui/button";

export type Option<T extends string | number = string> = { label: string; value: T };

interface SelectBoxWithCreateProps<T extends string | number = string> {
    options: Option<T>[];
    value: T | null | undefined;
    onChange: (value: T | null) => void;
    onCreate?: (label: string) => void;
    allowCreate?: boolean;
    placeholder?: string;
}

export function SelectBoxWithCreate<T extends string | number = string>({
                                        options,
                                        value,
                                        onChange,
                                        onCreate,
                                        allowCreate = true,
                                        placeholder = "Auswählen oder erstellen"
                                    }: SelectBoxWithCreateProps<T>) {
    const [open, setOpen] = React.useState(false);
    const [search, setSearch] = React.useState("");

    const selectedLabel = value ? (options.find(o => o.value === value)?.label ?? value) : "";
    const filtered = options.filter(o => o.label.toLowerCase().includes(search.toLowerCase()));

    return (
        <Popover open={open} onOpenChange={setOpen}>
            <PopoverTrigger asChild>
                <Button
                    variant="outline"
                    role="combobox"
                    aria-expanded={open}
                    className="w-full justify-between"
                >
                    {selectedLabel || placeholder}
                    <ChevronsUpDown className="ml-2 h-4 w-4 opacity-50"/>
                </Button>
            </PopoverTrigger>
            <PopoverContent className="p-0 w-[260px]">
                <Command shouldFilter={false}>
                    <CommandInput
                        placeholder="Suchen..."
                        value={search}
                        onValueChange={setSearch}
                    />
                    <CommandList>
                        {filtered.length > 0 && (
                            <CommandGroup>
                                {filtered.map((o) => (
                                    <CommandItem
                                        key={o.value}
                                        onSelect={() => {
                                            onChange(o.value);
                                            setOpen(false);
                                        }}
                                    >
                                        <Check
                                            className={`mr-2 h-4 w-4 ${
                                                value === o.value ? "opacity-100" : "opacity-0"
                                            }`}
                                        />
                                        {o.label}
                                    </CommandItem>
                                ))}
                            </CommandGroup>
                        )}

                        {allowCreate && filtered.length === 0 && search.length > 0 && (
                            <CommandGroup>
                                <CommandItem
                                    key="__create__"
                                    onSelect={() => {
                                        if (!onCreate) return;
                                        onCreate(search);
                                        // Only auto-select the newly created value if current options are string-valued
                                        const isStringValues = typeof (options[0]?.value) === "string";
                                        if (isStringValues) onChange(search as unknown as T);
                                        setOpen(false);
                                        setSearch("");
                                    }}
                                >
                                    <Plus className="mr-2 h-4 w-4" />
                                    "{search}" erstellen
                                </CommandItem>
                            </CommandGroup>
                        )}
                    </CommandList>
                </Command>
            </PopoverContent>
        </Popover>
    );
}
