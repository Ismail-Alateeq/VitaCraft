using Microsoft.SemanticKernel;
using System.Text.Json;
using VitaCraft.Interfaces;
using VitaCraft.Models.DTOs;

namespace VitaCraft.Services
{
    public class ResumeOpenAiService : IResumeOpenAiSrevice
    {
        private readonly Kernel _kernel;

        public ResumeOpenAiService(Kernel kernel)
        {
            _kernel = kernel;
        }

        public async Task<ResumeJsonDto> ParseResumeAsync(ResumeDTO dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            var prompt = @"
            You are a helpful AI assistant whose job is to take raw resume/ form data(provided below)
            and output a single, valid JSON object in English only. If any part of the input is in another
            language, translate it into English before proceeding.Use all details you can glean from the
            Summary field to craft a strong, concise personal headline and a powerful professional summary.
            Enhance or expand any sparse content to make it more impactful, but do not invent credentials the
            user never provided.

            Below is the EXACT schema for the JSON you must produce.Do not add any extra keys.Fill in
            every key; if a section has no data, use null(for single‐value fields) or an empty array(for lists).

Convert the following unstructured CV text into JSON with the structure:
{
            ""Title"": ""string"",
            ""FirstName"": ""string"",
            ""LastName"": ""string"",
            ""Email"": ""string"",
            ""PhoneNumber"": ""string"",
            ""LinkedinLink"": ""string or null"",
            ""GitHubLink"": ""string or null"",
            ""Summary"": ""string"",
            ""Educations"": [
            {
            ""CollegeName"": ""string"",
            ""IsCurrent"": ""boolean(or null)"",
            ""DegreeType"": ""string"",
            ""Major"": ""string"",
            ""StartDate"": ""string"",
            ""EndDate"": ""string"",
            ""GPA"": number(or null)
            }
            ],
            ""Experiences"": [
            {
            ""Title"": ""string"",
            ""Company"": ""string"",
            ""StartDate"": ""string"",
            ""EndDate"": ""string"",
            ""IsCurrent"": boolean(or null),
            ""Duties"": ""string""
            }
            ],
            ""Skills"": [
            { ""SkillName"": ""string"", ""SkillType"": ""string"" }, ...
            ]
            ],
            ""Certificates"": [
            {
            ""Title"": ""string"",
            ""ProviderName"": ""string"",
            ""Field"": ""string"",
            ""StartDate"": ""string"",
            ""EndDate"": ""string"",
            ""GPA"": number(or null)
            }
            ],
            }
IF any field is not present, use Not null as the value and if it was array use empty array [].
CV TEXT:
{{$input}}

JSON:
";

            // 2. Create a “single‐string” prompt function:


            //3.Build rawText by concatenating each DTO property, one per line:
            string rawText = $@"
                          FirstName: {dto.FirstName ?? ""}
                          LastName: {dto.LastName ?? ""}
                          Email: {dto.Email ?? ""}
                          Phone: {dto.Phone ?? ""}
                          LinkedinLink: {dto.LinkedinLink ?? ""}
                          GitHubLink: {dto.GitHubLink ?? ""}
                          Summary: {(dto.bio ?? "").Replace("\r\n", " ").Replace("\n", " ")}
                          Education: {(dto.Education ?? "").Replace("\r\n", "; ").Replace("\n", "; ")}
                          Experience: {(dto.Experience ?? "").Replace("\r\n", "; ").Replace("\n", "; ")}
                          Skills: {(dto.Skills ?? "").Replace("\r\n", ", ").Replace("\n", ", ")}
                          Certificate:{dto.Certificates ?? ""}
            ";

          

            KernelFunction extractFunction = _kernel.CreateFunctionFromPrompt(prompt);

            FunctionResult result = await _kernel.InvokeAsync(extractFunction, new()
            {
                ["input"] = rawText
            });


            string json = result.ToString();
            Console.WriteLine(json);


            ResumeJsonDto resumeDto = JsonSerializer.Deserialize<ResumeJsonDto>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new ResumeJsonDto();


            resumeDto.FirstName = resumeDto.FirstName ?? "";
            resumeDto.Title = resumeDto.Title ?? "";
            resumeDto.LastName = resumeDto.LastName ?? "";
            resumeDto.Email = resumeDto.Email ?? "";
            resumeDto.PhoneNumber = resumeDto.PhoneNumber;
            resumeDto.Address = resumeDto.Address ?? "";
            resumeDto.Summary = resumeDto.Summary ?? "";
            resumeDto.Title = resumeDto.Title ?? "";
            resumeDto.GitHubLink = resumeDto.GitHubLink ?? "";
            resumeDto.LinkedinLink = resumeDto.LinkedinLink ?? "";
            resumeDto.Educations ??= new List<EducationItem>();
            resumeDto.Experiences ??= new List<ExperienceItem>();
            resumeDto.Skills ??= new List<SkillItem1>();
            resumeDto.Certificates ??= new List<CertificateItem>();


            return resumeDto;
        }
    }
}
