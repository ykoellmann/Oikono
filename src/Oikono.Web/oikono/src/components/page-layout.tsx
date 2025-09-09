import { SidebarInset, SidebarProvider, SidebarTrigger } from "@/components/ui/sidebar.tsx";
import { Separator } from "@/components/ui/separator.tsx";
import { Breadcrumb, BreadcrumbItem, BreadcrumbList, BreadcrumbLink, BreadcrumbPage } from "@/components/ui/breadcrumb.tsx";
import { AppSidebar } from "@/components/app-sidebar.tsx";
import { Outlet, Link, useLocation } from "react-router-dom";

const NAME_MAP: Record<string, string> = {
    "": "Home",
    "recipes": "Recipes",
};

export default function PageLayout() {
    const location = useLocation();
    const segments = location.pathname.split("/").filter(Boolean);
    const crumbs = ["", ...segments];

    return (
        <SidebarProvider>
            <AppSidebar />
            <SidebarInset>
                <header className="flex h-10 md:h-12 shrink-0 items-center gap-2 border-b pl-2">
                    <div className="flex items-center gap-2 pl-0 pr-4">
                        <SidebarTrigger className="-ml-1" />
                        <Separator orientation="vertical" className="mr-2 h-4" />
                        <Breadcrumb>
                            <BreadcrumbList>
                                {crumbs.map((seg, idx) => {
                                    const href = "/" + crumbs.slice(1, idx + 1).join("/");
                                    const isLast = idx === crumbs.length - 1;
                                    const label = NAME_MAP[seg] ?? seg.charAt(0).toUpperCase() + seg.slice(1);
                                    return (
                                        <BreadcrumbItem key={idx}>
                                            {isLast ? (
                                                <BreadcrumbPage>{label || "Home"}</BreadcrumbPage>
                                            ) : (
                                                <BreadcrumbLink asChild>
                                                    <Link to={href || "/"}>{label || "Home"}</Link>
                                                </BreadcrumbLink>
                                            )}
                                        </BreadcrumbItem>
                                    );
                                })}
                            </BreadcrumbList>
                        </Breadcrumb>
                    </div>
                </header>
                <main className="flex flex-1 flex-col gap-2 p-0">
                    <Outlet />
                </main>
            </SidebarInset>
        </SidebarProvider>
    );
}