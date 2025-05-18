<?php
require 'config.php';

$stmt = $conn->prepare("SELECT * FROM dynamic");
$stmt->execute();
$result = $stmt->get_result();

$questions = array();
while ($row = $result->fetch_assoc()) {
    $questions[] = $row;
}

echo json_encode($questions);

$stmt->close();
$conn->close();
?>