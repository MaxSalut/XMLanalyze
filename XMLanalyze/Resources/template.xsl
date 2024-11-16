<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<xsl:output method="html" encoding="UTF-8" indent="yes" />

	<xsl:template match="/Results">
		<html>
			<head>
				<title>Результати пошуку</title>
				<style>
					body {
					font-family: Arial, sans-serif;
					margin: 20px;
					}
					h1 {
					text-align: center;
					color: #333;
					}
					table {
					width: 100%;
					border-collapse: collapse;
					margin-top: 20px;
					}
					th, td {
					border: 1px solid #ccc;
					padding: 10px;
					text-align: center;
					}
					th {
					background-color: #f4f4f4;
					font-weight: bold;
					color: #555;
					}
					tr:nth-child(even) {
					background-color: #f9f9f9;
					}
					tr:hover {
					background-color: #f1f1f1;
					}
				</style>
			</head>
			<body>
				<h1>Результати пошуку</h1>
				<table>
					<tr>
						<th>ПІБ</th>
						<th>Факультет</th>
						<th>Курс</th>
						<th>Кімната</th>
						<th>Дата заселення</th>
						<th>Дата виселення</th>
					</tr>
					<xsl:for-each select="Person">
						<tr>
							<td>
								<xsl:value-of select="@FullName" />
							</td>
							<td>
								<xsl:value-of select="@Faculty" />
							</td>
							<td>
								<xsl:value-of select="@Course" />
							</td>
							<td>
								<xsl:value-of select="Room" />
							</td>
							<td>
								<xsl:value-of select="Dates/CheckInDate" />
							</td>
							<td>
								<xsl:value-of select="Dates/CheckOutDate" />
							</td>
						</tr>
					</xsl:for-each>
				</table>
			</body>
		</html>
	</xsl:template>
</xsl:stylesheet>
