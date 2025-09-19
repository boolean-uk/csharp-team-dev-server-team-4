﻿using exercise.wwwapi.DTOs.Users;
using System.Text.Json.Serialization;

namespace exercise.wwwapi.DTOs.GetUsers;

public class UsersSuccessDTO
{
    [JsonPropertyName("users")] 
    public List<UserDTO> Users { get; set; } = [];
}