import {useEffect, useRef, useState} from "react"
import {Badge} from "@/components/ui/badge.tsx";


export function TagList({tags}: { tags: string[] }) {
    const containerRef = useRef<HTMLDivElement>(null)
    const [visibleCount, setVisibleCount] = useState(tags.length)

    useEffect(() => {
        const container = containerRef.current
        if (!container) return

        const resizeObserver = new ResizeObserver(() => {
            const badges = Array.from(container.children) as HTMLElement[]
            let lastVisible = badges.length

            for (let i = 0; i < badges.length; i++) {
                const badge = badges[i]
                if (badge.offsetTop > container.clientHeight - badge.offsetHeight) {
                    lastVisible = i
                    break
                }
            }

            setVisibleCount(lastVisible - 1) // -1, weil wir Platz für "+X" brauchen
        })

        resizeObserver.observe(container)
        return () => resizeObserver.disconnect()
    }, [tags])

    const hiddenCount = tags.length - visibleCount

    return (
        <div
            ref={containerRef}
        >
            {tags.slice(0, visibleCount).map((tag) => (
                <Badge key={tag} variant="secondary">
                    {tag}
                </Badge>
            ))}

            {hiddenCount > 0 && (
                <Badge variant="secondary">+{hiddenCount}</Badge>
            )}
        </div>
    )
}
