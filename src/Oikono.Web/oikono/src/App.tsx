import './App.css'
import PageLayout from "@/components/page-layout.tsx";
import { Route, Routes, Navigate } from 'react-router-dom'
import HomePage from '@/pages/home.tsx'
import RecipesPage from '@/pages/recipes/index.tsx'
import NotFoundPage from '@/pages/not-found.tsx'

function App() {
  return (
    <Routes>
      <Route element={<PageLayout />}>
        <Route index element={<HomePage />} />
        <Route path="recipes" element={<RecipesPage />} />
        <Route path="/home" element={<Navigate to="/" replace />} />
        <Route path="*" element={<NotFoundPage />} />
      </Route>
    </Routes>
  )
}

export default App
