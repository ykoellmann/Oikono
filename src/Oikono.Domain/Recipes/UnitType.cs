namespace Oikono.Domain.Recipes;

public enum UnitType
{
    // Volumen
    Milliliter,
    Liter,

    // Gewicht
    Gram,
    Kilogram,

    // Längen (für Teigstücke, Nudeln etc. eher selten, optional)
    Centimeter,
    Meter,

    // Allgemeine Küchenmaße
    Teaspoon,   // TL
    Tablespoon, // EL
    Piece       // Stück (z. B. 1 Ei, 3 Tomaten)
}