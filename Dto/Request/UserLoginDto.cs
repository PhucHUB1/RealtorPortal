﻿namespace RealEstate.Dto.Request;

public class UserLoginDto
{
    public string? UsernameOrEmail { get; set; }
    public string? Password { get; set; }
}