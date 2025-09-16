import * as React from "react";
import {Plus, ChevronsUpDown, Check} from "lucide-react";
import {Command, CommandGroup, CommandItem, CommandList, CommandInput} from "@/components/ui/command";
import {Popover, PopoverContent, PopoverTrigger} from "@/components/ui/popover";
import {Button} from "@/components/ui/button";

export type Option = { label: string; value: string | number };

interface SelectBoxWithCreateProps {
    options: Option[];
    value: string | number | null | undefined;
    onChange: (value: string | number | null) => void;
    onCreate?: (label: string) => void;
    allowCreate?: boolean;
    placeholder?: string;
}

export function SelectBoxWithCreate({
                                        options,
                                        value,
                                        onChange,
                                        onCreate,
                                        allowCreate = true,
                                        placeholder = "Auswählen oder erstellen"
                                    }: SelectBoxWithCreateProps) {
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
                                        onChange(search);
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
