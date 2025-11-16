using AutoMapper;
using GamifiedLearningPlatform.DTOs;
using GamifiedLearningPlatform.Models;

namespace GamifiedLearningPlatform.Mappers;

public static class StudentMapper
{
    public static readonly IMapper Mapper;

    static StudentMapper()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Assignment, AssignmentDto>().ReverseMap();


            cfg.CreateMap<Student, StudentDto>();

            cfg.CreateMap<StudentDto, Student>()
                .ForMember(dest => dest.FullName, opt => opt.Ignore());
        });

        Mapper = config.CreateMapper();
    }
}