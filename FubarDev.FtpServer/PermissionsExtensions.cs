﻿// <copyright file="PermissionsExtensions.cs" company="Fubar Development Junker">
// Copyright (c) Fubar Development Junker. All rights reserved.
// </copyright>

using System;

using FubarDev.FtpServer.AccountManagement;
using FubarDev.FtpServer.FileSystem;
using FubarDev.FtpServer.FileSystem.Generic;

using JetBrains.Annotations;

namespace FubarDev.FtpServer
{
    /// <summary>
    /// Extension methods for <see cref="IUnixPermissions"/>
    /// </summary>
    public static class PermissionsExtensions
    {
        /// <summary>
        /// Gets the effective access mode for an <paramref name="entity"/> for the given <paramref name="user"/>
        /// </summary>
        /// <param name="permissions">The permissions used to build the access mode</param>
        /// <param name="entity">The entity owner information</param>
        /// <param name="user">The FTP user to determine the access mode for</param>
        /// <returns>The effective access mode for the <paramref name="user"/></returns>
        [NotNull]
        public static IAccessMode GetAccessModeFor([NotNull] this IUnixPermissions permissions, [NotNull] IUnixOwner entity, [NotNull] FtpUser user)
        {
            var isUser = string.Equals(entity.Owner, user.Name, StringComparison.OrdinalIgnoreCase);
            var isGroup = user.IsInGroup(entity.Group);
            var canRead = (isUser && permissions.User.Read)
                          || (isGroup && permissions.Group.Read)
                          || permissions.Other.Read;
            var canWrite = (isUser && permissions.User.Write)
                           || (isGroup && permissions.Group.Write)
                           || permissions.Other.Write;
            var canExecute = (isUser && permissions.User.Execute)
                             || (isGroup && permissions.Group.Execute)
                             || permissions.Other.Execute;
            return new GenericAccessMode(canRead, canWrite, canExecute);
        }
    }
}
