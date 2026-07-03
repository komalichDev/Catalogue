using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseAccess.Repositorymodel;
public record Product(
    int Id, 
    string Name, 
    double Price, 
    Description Description, 
    Category Category);