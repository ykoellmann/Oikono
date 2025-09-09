import { Link } from "react-router-dom"

export default function NotFoundPage() {
  return (
    <div className="p-6">
      <h1 className="text-2xl font-semibold mb-2">Seite nicht gefunden</h1>
      <p className="mb-4 text-muted-foreground">Die angeforderte Seite existiert nicht.</p>
      <Link className="underline" to="/">Zur Startseite</Link>
    </div>
  )
}
