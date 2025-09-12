import PageLayout from "@/components/page-layout.tsx";

export default function HomePage() {
  return (
      <PageLayout title="Home" >
          <div className="p-4">
              <h1 className="text-2xl font-semibold">Willkommen</h1>
              <p className="text-muted-foreground">Wähle eine Seite in der Sidebar, z. B. Rezepte.</p>
          </div>
      </PageLayout>
  )
}
