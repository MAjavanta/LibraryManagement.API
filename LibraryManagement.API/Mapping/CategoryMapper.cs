using LibraryManagement.API.DTOs.Categories;
using LibraryManagement.API.Entities;
using Riok.Mapperly.Abstractions;

namespace LibraryManagement.API.Mapping;

[Mapper]
public partial class CategoryMapper
{
    public partial ReadCategoryDTO ToReadDto(Category source);

    [MapperIgnoreTarget(nameof(Category.Id))]
    public partial Category ToEntity(CreateCategoryDTO source);

    [MapperIgnoreTarget(nameof(Category.Id))]
    public partial void UpdateEntity(UpdateCategoryDTO source, Category target);
}
