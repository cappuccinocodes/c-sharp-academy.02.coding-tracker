using Coding_Tracker;
using Coding_Tracker.models;
using System;
using System.Globalization;

Database db = new();
db.Initialize();

var controller = new Controller();
var validation = new Validation();

var view = new View(controller, validation);
view.Start();