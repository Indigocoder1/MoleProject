using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PeriodicTable
{
    public class Element : MonoBehaviour
    {
        public int listIndex { get; set; }
        public int atomicNumber { get; set; }
        public string symbol { get; set; }
        public string elementName { get; set; }
        public float atomicMass { get; set; }

        public Color elementColor { get; set; }

        public Element(int atomicNumber, string symbol, string elementName, float atomicMass, Color? elementColor = null)
        {
            this.atomicNumber = atomicNumber;
            this.symbol = symbol;
            this.elementName = elementName;
            this.atomicMass = atomicMass;
            if(elementColor == null)
            {
                this.elementColor = Color.white;
            }
            else
            {
                this.elementColor = (Color)elementColor;
            }
        }
    }

    public static class Table
    {
        public static List<Element> elements = new List<Element>
        {
            new Element(1, "H", "Hydrogen", 1.01f),
            new Element(2, "He", "Helium", 4.00f),
            new Element(3, "Li", "Lithium", 6.94f),
            new Element(4, "Be", "Beryllium", 9.01f),
            new Element(5, "B", "Boron", 10.81f),
            new Element(6, "C", "Carbon", 12.01f),
            new Element(7, "N", "Nitrogen", 14.01f),
            new Element(8, "O", "Oxygen", 16.00f),
            new Element(9, "F", "Fluorine", 19.00f),
            new Element(10, "Ne", "Neon", 20.18f),
            new Element(11, "Na", "Sodium", 22.99f),
            new Element(12, "Mg", "Magnesium", 24.31f),
            new Element(13, "Al", "Aluminum", 26.98f),
            new Element(14, "Si", "Silicon", 28.09f),
            new Element(15, "P", "Phosphorus", 30.97f),
            new Element(16, "S", "Sulphur", 32.07f),
            new Element(17, "Cl", "Chlorine", 35.45f),
            new Element(18, "Ar", "Argon", 39.95f),
            new Element(19, "K", "Potassium", 39.01f),
            new Element(20, "Ca", "Calcium", 40.08f),
            new Element(21, "Sc", "Scandium", 44.98f),
            new Element(22, "Ti", "Titanium", 47.88f),
            new Element(23, "V", "Vanadium", 50.94f),
            new Element(24, "Cr", "Chromium", 52.00f),
            new Element(25, "Mn", "Manganese", 54.94f),
            new Element(26, "Fe", "Iron", 55.85f),
            new Element(27, "Co", "Cobalt", 58.93f),
            new Element(28, "Ni", "Nickel", 58.69f),
            new Element(29, "Cu", "Copper", 63.55f, new Color(252, 186, 3)),
            new Element(30, "Zn", "Zinc", 65.39f),
            new Element(31, "Ga", "Gallium", 69.72f),
            new Element(32, "Ge", "Germanium", 72.61f),
            new Element(33, "As", "Arsenic", 74.92f),
            new Element(34, "Se", "Selenium", 78.96f),
            new Element(35, "Br", "Bromine", 79.90f),
            new Element(36, "Kr", "Krypton", 83.80f),
            new Element(37, "Rb", "Rubidium", 85.47f),
            new Element(38, "Sr", "Strontium", 87.62f),
            new Element(39, "Y", "Yttrium", 88.91f),
            new Element(40, "Zr", "Zirconium", 91.22f),
            new Element(41, "Nb", "Niobium", 92.91f),
            new Element(42, "Mo", "Molybdenum", 95.94f),
            new Element(43, "Tc", "Technetium", 98f),
            new Element(44, "Ru", "Ruthenium", 101.07f),
            new Element(45, "Rh", "Rhodium", 102.91f),
            new Element(46, "Pd", "Palladium", 106.42f),
            new Element(47, "Ag", "Silver", 107.87f),
            new Element(48, "Cd", "Cadmium", 112.41f),
            new Element(49, "In", "Indium", 114.82f),
            new Element(50, "Sn", "Tin", 118.71f),
            new Element(51, "Sb", "Antimony", 121.76f),
            new Element(52, "Te", "Tellurium", 127.60f),
            new Element(53, "I", "Iodine", 126.90f),
            new Element(54, "Xe", "Xenon", 131.29f),
            new Element(55, "Cs", "Cesium", 132.91f),
            new Element(56, "Ba", "Barium", 137.33f),
            new Element(57, "La", "Lanthanum", 138.91f),
            new Element(58, "Ce", "Cerium", 140.12f),
            new Element(59, "Pr", "Praseodymium", 140.91f),
            new Element(60, "Nd", "Neodymium", 144.24f),
            new Element(61, "Pm", "Promethium", 145f),
            new Element(62, "Sm", "Samarium", 150.36f),
            new Element(63, "Eu", "Europium", 151.97f),
            new Element(64, "Gd", "Gadolinium", 157.25f),
            new Element(65, "Tb", "Terbium", 158.93f),
            new Element(66, "Dy", "Dysprosium", 162.50f),
            new Element(67, "Ho", "Holmium", 164.93f),
            new Element(68, "Er", "Erbium", 167.26f),
            new Element(69, "Tm", "Thulium", 168.93f),
            new Element(70, "Yb", "Ytterbium", 173.04f),
            new Element(71, "Lu", "Lutetium", 174.97f),
            new Element(72, "Hf", "Hafnium", 178.49f),
            new Element(73, "Ta", "Tantalum", 180.95f),
            new Element(74, "W", "Tungsten", 183.84f),
            new Element(75, "Re", "Rhenium", 186.21f),
            new Element(76, "Os", "Osmium", 190.23f),
            new Element(77, "Ir", "Iridium", 192.22f),
            new Element(78, "Pt", "Platinum", 195.08f),
            new Element(79, "Au", "Gold", 196.97f),
            new Element(80, "Hg", "Mercury", 200.59f),
            new Element(81, "Tl", "Thallium", 204.38f),
            new Element(82, "Pb", "Lead", 207.20f),
            new Element(83, "Bi", "Bismuth", 208.98f),
            new Element(84, "Po", "Polonium", 209f),
            new Element(85, "At", "Astatine", 210f),
            new Element(86, "Rn", "Radon", 222f),
            new Element(87, "Fr", "Francium", 233f),
            new Element(88, "Ra", "Radium", 226f),
            new Element(89, "Ac", "Actinium", 227f),
            new Element(90, "Th", "Thorium", 232.04f),
            new Element(91, "Pa", "Protactinium", 231.04f),
            new Element(92, "U", "Uranium", 238.03f),
            new Element(93, "Np", "Neptunium", 237f),
            new Element(94, "Pu", "Plutonium", 244f),
            new Element(95, "Am", "Americium", 243f),
            new Element(96, "Cm", "Curium", 247f),
            new Element(97, "Bk", "Berkelium", 247f),
            new Element(98, "Cf", "Californium", 251f),
            new Element(99, "Es", "Einsteinium", 254f),
            new Element(100, "Fm", "Fermium", 257f),
            new Element(101, "Md", "Mendelevium", 258f),
            new Element(102, "No", "Nobelium", 259f),
            new Element(103, "Lr", "Lawrencium", 262f),
            new Element(104, "Rf", "Rutherfordium", 267f),
            new Element(105, "Db", "Dubnium", 268f),
            new Element(106, "Sg", "Seaborgium", 261f),
            new Element(107, "Bh", "Bohrium", 272f),
            new Element(108, "Hs", "Hassium", 270f),
            new Element(109, "Mt", "Meitnerium", 276f),
            new Element(110, "Ds", "Darmstadtium", 261f),
            new Element(111, "Rg", "Roentgenium", 280f),
            new Element(112, "Cn", "Copernicium", 285f),
            new Element(113, "Uut", "Unutrium", 284f),
            new Element(114, "Uuq", "Ununquadium", 289f),
            new Element(115, "Uup", "Unupentium", 288f)
        };
    }
}
