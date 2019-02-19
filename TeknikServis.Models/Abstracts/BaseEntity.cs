﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeknikServis.Models.Abstracts
{
    public abstract class RepositoryBase<T>
    {
        [Key]
        [Column(Order = 1)]
        public T Id { get; set; }

    }
}