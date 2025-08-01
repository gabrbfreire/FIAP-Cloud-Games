using System.ComponentModel.DataAnnotations;

namespace FiapCloudGames.API.DTOs;

public class CadastrarPromocaoDto
{
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [MaxLength(50, ErrorMessage = "O nome deve ter no máximo 50 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O percentual de desconto é obrigatório.")]
    [Range(1, 100, ErrorMessage = "O percentual de desconto deve ser entre 1 e 100.")]
    public int PercentualDeDesconto { get; set; }

    [Required(ErrorMessage = "A data de início é obrigatória.")]
    public DateTime DataInicio { get; set; }

    [Required(ErrorMessage = "A data de fim é obrigatória.")]
    [CustomValidation(typeof(CadastrarPromocaoDto), "ValidateDatas")]
    public DateTime DataFim { get; set; }

    public static ValidationResult ValidateDatas(DateTime dataFim, ValidationContext context)
    {
        var instance = (CadastrarPromocaoDto)context.ObjectInstance;

        if (instance.DataInicio >= instance.DataFim)
        {
            return new ValidationResult("A data de início deve ser menor que a data de fim.");
        }

        if ((instance.DataFim - instance.DataInicio).TotalDays < 1)
        {
            return new ValidationResult("A diferença entre as datas deve ser de no mínimo 1 dia.");
        }

        return ValidationResult.Success;
    }
}