<?php

function update($src,$val,$level) {
  $rootdir = "/monitor/data";
  $date = new DateTime();
  $time = $date->format('Y-m-d H:i:s');
  $src_regex = '/^[A-Za-z0-9][-_A-Za-z0-9]*[A-Za-z0-9](\.[A-Za-z0-9][-_A-Za-z0-9]*[A-Za-z0-9]+)*$/';
  if (preg_match($src_regex, $src)) {
    $file = $rootdir . "/". strtolower($src) . ".txt";

    $fp = fopen($file, "w");
    fwrite($fp,$time . "\n");
    fwrite($fp,$level . "\n");
    fwrite($fp,$val . "\n");
    fclose($fp);  
  } else {
    die("Invalid");
  }
}

?>
