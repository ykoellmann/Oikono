import * as React from "react";
import { useMediaQuery } from "@/hooks/use-media-query";
import {
    Dialog,
    DialogContent,
    DialogHeader,
    DialogTitle,
    DialogDescription,
    DialogTrigger,
    DialogClose,
    DialogFooter,
} from "@/components/ui/dialog";
import {
    Drawer, DrawerClose,
    DrawerContent,
    DrawerDescription, DrawerFooter,
    DrawerHeader,
    DrawerTitle,
    DrawerTrigger
} from "@/components/ui/drawer.tsx";

// gemeinsame Typisierung: alle Props von Dialog oder Drawer
type PopupProps = {
    open: boolean;
    onOpenChange: (open: boolean) => void;
    children: React.ReactNode;
    mode?: "auto" | "dialog" | "drawer"; // optionaler Override
};

export function Popup({ open, onOpenChange, children, mode = "auto" }: PopupProps) {
    const isDesktop = useMediaQuery("(min-width: 768px)");
    const useDialog = mode === "dialog" || (mode === "auto" && isDesktop);

    if (useDialog) {
        return <Dialog open={open} onOpenChange={onOpenChange}>{children}</Dialog>;
    }
    return <Drawer open={open} onOpenChange={onOpenChange}>{children}</Drawer>;
}

// Sub-Komponenten, die je nach Modus switchen
export function PopupTrigger({ children, ...props }: { children: React.ReactNode; [key: string]: any }) {
    const isDesktop = useMediaQuery("(min-width: 768px)");
    return isDesktop ? (
        <DialogTrigger {...props}>{children}</DialogTrigger>
    ) : (
        <DrawerTrigger {...props}>{children}</DrawerTrigger>
    );
};

export function PopupContent({ children, ...props }: { children: React.ReactNode; [key: string]: any }) {
    const isDesktop = useMediaQuery("(min-width: 768px)");
    return isDesktop ? (
        <DialogContent {...props}>{children}</DialogContent>
    ) : (
        <DrawerContent {...props}>{children}</DrawerContent>
    );
};

export function PopupHeader({ children, ...props }: { children: React.ReactNode; [key: string]: any }) {
    const isDesktop = useMediaQuery("(min-width: 768px)");
    return isDesktop ? (
        <DialogHeader {...props}>{children}</DialogHeader>
    ) : (
        <DrawerHeader {...props}>{children}</DrawerHeader>
    );
};

export function PopupTitle({ children, ...props }: { children: React.ReactNode; [key: string]: any }) {
    const isDesktop = useMediaQuery("(min-width: 768px)");
    return isDesktop ? (
        <DialogTitle {...props}>{children}</DialogTitle>
    ) : (
        <DrawerTitle {...props}>{children}</DrawerTitle>
    );
};

export function PopupDescription({ children, ...props }: { children: React.ReactNode; [key: string]: any }) {
    const isDesktop = useMediaQuery("(min-width: 768px)");
    return isDesktop ? (
        <DialogDescription {...props}>{children}</DialogDescription>
    ) : (
        <DrawerDescription {...props}>{children}</DrawerDescription>
    );
};

export function PopupClose({ children, ...props }: { children: React.ReactNode; [key: string]: any }) {
    const isDesktop = useMediaQuery("(min-width: 768px)");
    return isDesktop ? (
        <DialogClose {...props}>{children}</DialogClose>
    ) : (
        <DrawerClose {...props}>{children}</DrawerClose>
    );
};

export function PopupFooter({ children, ...props }: { children: React.ReactNode; [key: string]: any }) {
    const isDesktop = useMediaQuery("(min-width: 768px)");
    return isDesktop ? (
        <DialogFooter {...props}>{children}</DialogFooter>
    ) : (
        <DrawerFooter {...props}>{children}</DrawerFooter>
    );
};
