﻿using UniversityTimetable.Shared.Interfaces.Data;
using UniversityTimetable.Shared.Interfaces.ModelsRelationships;

namespace UniversityTimetable.Shared.Models.RelationModels
{
    public class SubjectTeacher : IModel, IIsDeleted, IRelationModel<Teacher, Subject>, IRelationModel<Subject, Teacher>
    {
        public int Id { get; set; }
        public int TeacherId { get; set; }
        public int SubjectId { get; set; }

        public Teacher Teacher { get; set; }
        public Subject Subject { get; set; }
        public bool IsDeleted { get; set; }

        int IRelationModel<Subject, Teacher>.RightTableId { get => SubjectId; set => SubjectId = value; }
        int IRelationModel<Subject, Teacher>.LeftTableId { get => TeacherId; set => TeacherId = value; }
        int IRelationModel<Teacher, Subject>.RightTableId { get => TeacherId; set => TeacherId = value; }
        int IRelationModel<Teacher, Subject>.LeftTableId { get => SubjectId; set => SubjectId = value; }
    }
}