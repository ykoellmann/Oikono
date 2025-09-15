import * as React from "react";

export function useMediaQuery(query: string) {
    const [matches, setMatches] = React.useState<boolean>(() => typeof window !== "undefined" && window.matchMedia(query).matches);
    React.useEffect(() => {
        if (typeof window === "undefined") return;
        const mql = window.matchMedia(query);
        const handler = (e: MediaQueryListEvent) => setMatches(e.matches);
        setMatches(mql.matches);
        mql.addEventListener("change", handler);
        return () => mql.removeEventListener("change", handler);
    }, [query]);
    return matches;
}