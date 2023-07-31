<?php
ini_set('display_errors',1);
ini_set('display_startup_errors',1);
error_reporting(-1);
include "inc/monitor.inc.php";

$pwd = "ServMonSecret";

if (!(isset($_GET["src"]) && isset($_GET["val"]) && isset($_GET["level"])  && isset($_GET["secret"]))) {
  echo "UNDEF";
  exit(3);
}

$src = $_GET["src"];
$val = $_GET["val"];
$level = $_GET["level"];
$secret = $_GET["secret"];

if ($secret == $pwd) {
  update($src,$val,$level);
}

?>
