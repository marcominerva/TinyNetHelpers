﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TinyNetHelpers.EntityFrameworkCore.Extensions
{
    public static class DbContextExtensions
    {
        public static Task ExecuteTransactionAsync(this DbContext context, Func<Task> action, CancellationToken cancellationToken = default)
        {
            var strategy = context.Database.CreateExecutionStrategy();
            return strategy.ExecuteAsync(async () =>
            {
                using var transaction = await context.Database.BeginTransactionAsync(cancellationToken).ConfigureAwait(false);

                await action.Invoke().ConfigureAwait(false);
                await transaction.CommitAsync(cancellationToken).ConfigureAwait(false);
            });
        }
    }
}
