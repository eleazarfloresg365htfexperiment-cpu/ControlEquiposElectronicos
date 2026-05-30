namespace ControlEquiposElectronicos.ViewModels.Checklist;

public static class ChecklistEstados
{
    public static readonly string[] Revision =
    {
        "Correcto",
        "Con problema",
        "No aplica",
        "No revisado"
    };

    public static string CalcularResultadoGeneral(IEnumerable<string> estadosRevision)
    {
        var estados = estadosRevision.ToList();

        if (estados.Count == 0 || estados.All(e => e == "No revisado"))
            return "Pendiente";

        if (estados.Any(e => e == "Con problema"))
            return "Revisado con problemas";

        if (estados.All(e => e is "Correcto" or "No aplica"))
            return "Revisado correctamente";

        return "Revisado con observaciones";
    }

    public static string ResumenProgresoEquipo(IEnumerable<string> estadosRevision)
    {
        var estados = estadosRevision.ToList();
        var revisados = estados.Count(e => e != "No revisado");
        return $"{revisados}/{estados.Count} aspectos";
    }
}
