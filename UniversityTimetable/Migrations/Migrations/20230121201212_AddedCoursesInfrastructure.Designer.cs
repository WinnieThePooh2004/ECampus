﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UniversityTimetable.Infrastructure;

#nullable disable

namespace Migrations.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230121201212_AddedCoursesInfrastructure")]
    partial class AddedCoursesInfrastructure
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("UniversityTimetable.Shared.Models.Auditory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Building")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Auditories");
                });

            modelBuilder.Entity("UniversityTimetable.Shared.Models.Class", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AuditoryId")
                        .HasColumnType("int");

                    b.Property<int>("ClassType")
                        .HasColumnType("int");

                    b.Property<int>("DayOfWeek")
                        .HasColumnType("int");

                    b.Property<int>("GroupId")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.Property<int>("SubjectId")
                        .HasColumnType("int");

                    b.Property<int>("TeacherId")
                        .HasColumnType("int");

                    b.Property<int>("WeekDependency")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AuditoryId");

                    b.HasIndex("GroupId");

                    b.HasIndex("SubjectId");

                    b.HasIndex("TeacherId");

                    b.ToTable("Classes");
                });

            modelBuilder.Entity("UniversityTimetable.Shared.Models.Course", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int?>("SubjectId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SubjectId");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("UniversityTimetable.Shared.Models.CourseTask", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Deadline")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int>("MaxPoints")
                        .HasColumnType("int");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<bool>("ValidAfterDeadline")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.ToTable("CourseTasks");
                });

            modelBuilder.Entity("UniversityTimetable.Shared.Models.Department", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("FacultyId")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("FacultyId");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("UniversityTimetable.Shared.Models.Faculty", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Faculties");
                });

            modelBuilder.Entity("UniversityTimetable.Shared.Models.Group", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("DepartmentId")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("UniversityTimetable.Shared.Models.RelationModels.CourseGroup", b =>
                {
                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.Property<int>("GroupId")
                        .HasColumnType("int");

                    b.HasKey("CourseId", "GroupId");

                    b.HasIndex("GroupId");

                    b.ToTable("CourseGroups");
                });

            modelBuilder.Entity("UniversityTimetable.Shared.Models.RelationModels.CourseTeacher", b =>
                {
                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.Property<int>("TeacherId")
                        .HasColumnType("int");

                    b.HasKey("CourseId", "TeacherId");

                    b.HasIndex("TeacherId");

                    b.ToTable("CourseTeachers");
                });

            modelBuilder.Entity("UniversityTimetable.Shared.Models.RelationModels.SubjectTeacher", b =>
                {
                    b.Property<int>("TeacherId")
                        .HasColumnType("int");

                    b.Property<int>("SubjectId")
                        .HasColumnType("int");

                    b.HasKey("TeacherId", "SubjectId");

                    b.HasIndex("SubjectId");

                    b.ToTable("SubjectTeachers");
                });

            modelBuilder.Entity("UniversityTimetable.Shared.Models.RelationModels.UserAuditory", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("AuditoryId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "AuditoryId");

                    b.HasIndex("AuditoryId");

                    b.ToTable("UserAuditories", (string)null);
                });

            modelBuilder.Entity("UniversityTimetable.Shared.Models.RelationModels.UserGroup", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("GroupId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "GroupId");

                    b.HasIndex("GroupId");

                    b.ToTable("UserGroups", (string)null);
                });

            modelBuilder.Entity("UniversityTimetable.Shared.Models.RelationModels.UserTeacher", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("TeacherId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "TeacherId");

                    b.HasIndex("TeacherId");

                    b.ToTable("UserTeachers", (string)null);
                });

            modelBuilder.Entity("UniversityTimetable.Shared.Models.Student", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("GroupId")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.HasIndex("UserId")
                        .IsUnique()
                        .HasFilter("[UserId] IS NOT NULL");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("UniversityTimetable.Shared.Models.Subject", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Subjects");
                });

            modelBuilder.Entity("UniversityTimetable.Shared.Models.TaskSubmission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CourseTaskId")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int>("StudentId")
                        .HasColumnType("int");

                    b.Property<int>("TotalPoints")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CourseTaskId");

                    b.HasIndex("StudentId");

                    b.ToTable("TaskSubmissions");
                });

            modelBuilder.Entity("UniversityTimetable.Shared.Models.Teacher", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("DepartmentId")
                        .HasColumnType("int");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ScienceDegree")
                        .HasColumnType("int");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.HasIndex("UserId")
                        .IsUnique()
                        .HasFilter("[UserId] IS NOT NULL");

                    b.ToTable("Teachers");
                });

            modelBuilder.Entity("UniversityTimetable.Shared.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<int?>("StudentId")
                        .HasColumnType("int");

                    b.Property<int?>("TeacherId")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("UniversityTimetable.Shared.Models.Class", b =>
                {
                    b.HasOne("UniversityTimetable.Shared.Models.Auditory", "Auditory")
                        .WithMany("Classes")
                        .HasForeignKey("AuditoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("UniversityTimetable.Shared.Models.Group", "Group")
                        .WithMany("Classes")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("UniversityTimetable.Shared.Models.Subject", "Subject")
                        .WithMany("Classes")
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("UniversityTimetable.Shared.Models.Teacher", "Teacher")
                        .WithMany("Classes")
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Auditory");

                    b.Navigation("Group");

                    b.Navigation("Subject");

                    b.Navigation("Teacher");
                });

            modelBuilder.Entity("UniversityTimetable.Shared.Models.Course", b =>
                {
                    b.HasOne("UniversityTimetable.Shared.Models.Subject", "Subject")
                        .WithMany("Courses")
                        .HasForeignKey("SubjectId");

                    b.Navigation("Subject");
                });

            modelBuilder.Entity("UniversityTimetable.Shared.Models.CourseTask", b =>
                {
                    b.HasOne("UniversityTimetable.Shared.Models.Course", "Course")
                        .WithMany("Tasks")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");
                });

            modelBuilder.Entity("UniversityTimetable.Shared.Models.Department", b =>
                {
                    b.HasOne("UniversityTimetable.Shared.Models.Faculty", "Faculty")
                        .WithMany("Departments")
                        .HasForeignKey("FacultyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Faculty");
                });

            modelBuilder.Entity("UniversityTimetable.Shared.Models.Group", b =>
                {
                    b.HasOne("UniversityTimetable.Shared.Models.Department", "Department")
                        .WithMany("Groups")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Department");
                });

            modelBuilder.Entity("UniversityTimetable.Shared.Models.RelationModels.CourseGroup", b =>
                {
                    b.HasOne("UniversityTimetable.Shared.Models.Course", "Course")
                        .WithMany("CourseGroups")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("UniversityTimetable.Shared.Models.Group", "Group")
                        .WithMany("CourseGroups")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("Group");
                });

            modelBuilder.Entity("UniversityTimetable.Shared.Models.RelationModels.CourseTeacher", b =>
                {
                    b.HasOne("UniversityTimetable.Shared.Models.Course", "Course")
                        .WithMany("CourseTeachers")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("UniversityTimetable.Shared.Models.Teacher", "Teacher")
                        .WithMany("CourseTeachers")
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("Teacher");
                });

            modelBuilder.Entity("UniversityTimetable.Shared.Models.RelationModels.SubjectTeacher", b =>
                {
                    b.HasOne("UniversityTimetable.Shared.Models.Subject", "Subject")
                        .WithMany("TeacherIds")
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("UniversityTimetable.Shared.Models.Teacher", "Teacher")
                        .WithMany("SubjectIds")
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Subject");

                    b.Navigation("Teacher");
                });

            modelBuilder.Entity("UniversityTimetable.Shared.Models.RelationModels.UserAuditory", b =>
                {
                    b.HasOne("UniversityTimetable.Shared.Models.Auditory", "Auditory")
                        .WithMany("UsersIds")
                        .HasForeignKey("AuditoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("UniversityTimetable.Shared.Models.User", "User")
                        .WithMany("SavedAuditoriesIds")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Auditory");

                    b.Navigation("User");
                });

            modelBuilder.Entity("UniversityTimetable.Shared.Models.RelationModels.UserGroup", b =>
                {
                    b.HasOne("UniversityTimetable.Shared.Models.Group", "Group")
                        .WithMany("UsersIds")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("UniversityTimetable.Shared.Models.User", "User")
                        .WithMany("SavedGroupsIds")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");

                    b.Navigation("User");
                });

            modelBuilder.Entity("UniversityTimetable.Shared.Models.RelationModels.UserTeacher", b =>
                {
                    b.HasOne("UniversityTimetable.Shared.Models.Teacher", "Teacher")
                        .WithMany("UsersIds")
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("UniversityTimetable.Shared.Models.User", "User")
                        .WithMany("SavedTeachersIds")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Teacher");

                    b.Navigation("User");
                });

            modelBuilder.Entity("UniversityTimetable.Shared.Models.Student", b =>
                {
                    b.HasOne("UniversityTimetable.Shared.Models.Group", "Group")
                        .WithMany("Students")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("UniversityTimetable.Shared.Models.User", "User")
                        .WithOne("Student")
                        .HasForeignKey("UniversityTimetable.Shared.Models.Student", "UserId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Group");

                    b.Navigation("User");
                });

            modelBuilder.Entity("UniversityTimetable.Shared.Models.TaskSubmission", b =>
                {
                    b.HasOne("UniversityTimetable.Shared.Models.CourseTask", "CourseTask")
                        .WithMany("Submissions")
                        .HasForeignKey("CourseTaskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("UniversityTimetable.Shared.Models.Student", "Student")
                        .WithMany("Submissions")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CourseTask");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("UniversityTimetable.Shared.Models.Teacher", b =>
                {
                    b.HasOne("UniversityTimetable.Shared.Models.Department", "Department")
                        .WithMany("Teachers")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("UniversityTimetable.Shared.Models.User", "User")
                        .WithOne("Teacher")
                        .HasForeignKey("UniversityTimetable.Shared.Models.Teacher", "UserId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Department");

                    b.Navigation("User");
                });

            modelBuilder.Entity("UniversityTimetable.Shared.Models.Auditory", b =>
                {
                    b.Navigation("Classes");

                    b.Navigation("UsersIds");
                });

            modelBuilder.Entity("UniversityTimetable.Shared.Models.Course", b =>
                {
                    b.Navigation("CourseGroups");

                    b.Navigation("CourseTeachers");

                    b.Navigation("Tasks");
                });

            modelBuilder.Entity("UniversityTimetable.Shared.Models.CourseTask", b =>
                {
                    b.Navigation("Submissions");
                });

            modelBuilder.Entity("UniversityTimetable.Shared.Models.Department", b =>
                {
                    b.Navigation("Groups");

                    b.Navigation("Teachers");
                });

            modelBuilder.Entity("UniversityTimetable.Shared.Models.Faculty", b =>
                {
                    b.Navigation("Departments");
                });

            modelBuilder.Entity("UniversityTimetable.Shared.Models.Group", b =>
                {
                    b.Navigation("Classes");

                    b.Navigation("CourseGroups");

                    b.Navigation("Students");

                    b.Navigation("UsersIds");
                });

            modelBuilder.Entity("UniversityTimetable.Shared.Models.Student", b =>
                {
                    b.Navigation("Submissions");
                });

            modelBuilder.Entity("UniversityTimetable.Shared.Models.Subject", b =>
                {
                    b.Navigation("Classes");

                    b.Navigation("Courses");

                    b.Navigation("TeacherIds");
                });

            modelBuilder.Entity("UniversityTimetable.Shared.Models.Teacher", b =>
                {
                    b.Navigation("Classes");

                    b.Navigation("CourseTeachers");

                    b.Navigation("SubjectIds");

                    b.Navigation("UsersIds");
                });

            modelBuilder.Entity("UniversityTimetable.Shared.Models.User", b =>
                {
                    b.Navigation("SavedAuditoriesIds");

                    b.Navigation("SavedGroupsIds");

                    b.Navigation("SavedTeachersIds");

                    b.Navigation("Student");

                    b.Navigation("Teacher");
                });
#pragma warning restore 612, 618
        }
    }
}