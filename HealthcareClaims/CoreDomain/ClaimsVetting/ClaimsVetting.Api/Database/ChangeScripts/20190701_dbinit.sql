DROP TABLE IF EXISTS batches;
DROP TABLE IF EXISTS batch_vetting_statuses;

CREATE TABLE batch_vetting_statuses (
	id INT PRIMARY KEY,
	name VARCHAR (200) NOT NULL
);

CREATE TABLE batches (
	id UUID PRIMARY KEY,
	batch_vetting_status_id INT NOT NULL,
	batch_uri VARCHAR (2000) NOT NULL,
	vetting_report_uri VARCHAR (2000) NULL,
	creation_date TIMESTAMP NOT NULL,

	FOREIGN KEY (batch_vetting_status_id) REFERENCES batch_vetting_statuses (id)
);


