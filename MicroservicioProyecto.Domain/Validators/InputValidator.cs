using System.Text.RegularExpressions;

namespace MicroservicioProyecto.Domain.Validators
{
    /// <summary>
    /// Validador centralizado para prevenir inyecciones y validar datos de entrada
    /// </summary>
    public static class InputValidator
    {
        // Patrones peligrosos de SQL Injection
        private static readonly string[] SqlInjectionPatterns = new[]
        {
            @"(\bOR\b|\bAND\b).*=.*",
      @"';|--;|\/\*|\*\/",
            @"\bEXEC\b|\bEXECUTE\b",
            @"\bDROP\b|\bDELETE\b|\bUPDATE\b|\bINSERT\b",
    @"\bSELECT\b.*\bFROM\b",
  @"\bUNION\b.*\bSELECT\b",
          @"xp_cmdshell",
       @"\bSCRIPT\b.*>",
     @"<\s*script",
       @"javascript:",
            @"onerror\s*=",
   @"onload\s*="
        };

        public static string SanitizeString(string? input)
     {
      if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

 input = input.Trim();
          input = Regex.Replace(input, @"\s+", " ");
            return input;
        }

        public static string SanitizeName(string? input)
     {
    if (string.IsNullOrWhiteSpace(input))
         return string.Empty;

input = SanitizeString(input);

 if (!Regex.IsMatch(input, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s\-']+$"))
       {
   throw new ArgumentException($"El nombre '{input}' contiene caracteres no permitidos.");
            }

       return input;
    }

        public static bool ContainsSqlInjection(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
    return false;

            input = input.ToUpper();

            foreach (var pattern in SqlInjectionPatterns)
    {
                if (Regex.IsMatch(input, pattern, RegexOptions.IgnoreCase))
           return true;
       }

 return false;
}

        public static string ValidateAndSanitize(string? input, string fieldName)
  {
       if (string.IsNullOrWhiteSpace(input))
    return string.Empty;

  input = SanitizeString(input);

     if (ContainsSqlInjection(input))
   {
                throw new ArgumentException($"El campo '{fieldName}' contiene caracteres o patrones no permitidos.");
    }

 return input;
      }

        public static string SanitizeText(string? input)
        {
    if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            input = SanitizeString(input);

// Remover caracteres peligrosos para XSS
   input = Regex.Replace(input, @"<script[^>]*>.*?</script>", "", RegexOptions.IgnoreCase);
   input = Regex.Replace(input, @"javascript:", "", RegexOptions.IgnoreCase);
            input = Regex.Replace(input, @"on\w+\s*=", "", RegexOptions.IgnoreCase);

            if (ContainsSqlInjection(input))
       {
          throw new ArgumentException("El texto contiene patrones no permitidos.");
            }

            return input;
        }

    public static DateTime ValidateDate(DateTime date, bool canBePast = false, bool canBeFuture = true)
        {
       var today = DateTime.Now.Date;
            var maxFutureDate = today.AddYears(10);

            if (!canBePast && date.Date < today)
       {
      throw new ArgumentException("La fecha no puede ser anterior a hoy.");
     }

 if (!canBeFuture && date.Date > today)
      {
      throw new ArgumentException("La fecha no puede ser futura.");
  }

 if (date > maxFutureDate)
            {
         throw new ArgumentException("La fecha está demasiado lejos en el futuro (máximo 10 años).");
            }

       if (date.Year < 1900)
      {
        throw new ArgumentException("La fecha no es válida.");
       }

         return date;
        }

     public static void ValidateDateRange(DateTime? startDate, DateTime? endDate)
        {
     if (startDate.HasValue && endDate.HasValue)
  {
  if (endDate.Value.Date < startDate.Value.Date)
    {
    throw new ArgumentException("La fecha de fin no puede ser anterior a la fecha de inicio.");
    }

       if (endDate.Value.Date == startDate.Value.Date)
      {
       throw new ArgumentException("La fecha de fin no puede ser igual a la fecha de inicio.");
                }
        }
        }
    }
}
