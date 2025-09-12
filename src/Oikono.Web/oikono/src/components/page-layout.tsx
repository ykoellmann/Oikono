import { SidebarInset, SidebarProvider, SidebarTrigger } from "@/components/ui/sidebar.tsx";
import { Separator } from "@/components/ui/separator.tsx";
import { AppSidebar } from "@/components/app-sidebar.tsx";
import {ModeToggle} from "@/components/mode-toggle.tsx";
import {ThemeProvider} from "@/components/theme-provider.tsx";

type PageLayoutProps = {
    title: string
    children: React.ReactNode
}

export default function PageLayout({title, children}: PageLayoutProps) {

    return (
        <ThemeProvider defaultTheme="dark" storageKey="vite-ui-theme">
            <SidebarProvider>
                <AppSidebar />
                <SidebarInset>
                    <header className="flex h-10 md:h-12 shrink-0 items-center gap-2 border-b pl-2">
                        <div className="flex items-center gap-2 pl-0 pr-4">
                            <SidebarTrigger className="-ml-1" />
                            <Separator orientation="vertical" className="mr-2 h-4" />
                            <span className="text-sm font-medium">{title}</span>
                        </div>
                        <div className="flex flex-1 items-center justify-end">
                            <ModeToggle />
                        </div>
                    </header>
                    <main className="flex flex-1 flex-col gap-2 p-5 ">
                        {children}
                    </main>
                </SidebarInset>
            </SidebarProvider>
        </ThemeProvider>
    );
}